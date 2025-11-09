namespace ProApi.Application.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
