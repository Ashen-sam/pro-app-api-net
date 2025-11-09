using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProApi.Application.DTOs;
using ProApi.Application.Interfaces;
using ProApi.Domain.Entities;
using ProApi.Infrastructure.Data;

namespace ProApi.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly string _jwtKey;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _jwtKey = config["Jwt:Key"]!;
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || user.Password != request.Password)
                return null;

            var token = GenerateJwtToken(user);
            return new AuthResponse
            {
                Token = token,
                Email = user.Email,
                Name = user.Name
            };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existing = await _context.Users.AnyAsync(u => u.Email == request.Email);
            if (existing)
                throw new Exception("User already exists");

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password // you can hash this later
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            return new AuthResponse
            {
                Token = token,
                Email = user.Email,
                Name = user.Name
            };
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("id", user.UserId.ToString()),
                new Claim("name", user.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
