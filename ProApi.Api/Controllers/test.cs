using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace ProApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _config;

        public TestController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("dbtest")]
        public IActionResult TestDatabase()
        {
            var connString = _config.GetConnectionString("SupabaseConnection");

            try
            {
                using var conn = new NpgsqlConnection(connString);
                conn.Open();
                return Ok("✅ Database connection successful!");
            }
            catch (Exception ex)
            {
                return BadRequest($"❌ Connection failed: {ex.Message}");
            }
        }
    }
}
