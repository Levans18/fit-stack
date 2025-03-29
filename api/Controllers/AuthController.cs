
using FitStack.API.DTOs;
using FitStack.API.Data;
using FitStack.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitStack.API.Controllers
{

    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _hasher = new();

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(user => user.Username == dto.Username || user.Email == dto.Email))
                return BadRequest("Username or Email already Exists");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = "" //Will be set next
            };

            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Username == dto.UsernameOrEmail || user.Email == dto.UsernameOrEmail);

            if (user == null)
                return Unauthorized("Invalid username or email.");
            
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result != PasswordVerificationResult.Success)
                return Unauthorized("Invalid password");

            //Placeholder: JWT generation will go here later
            return Ok(new { message = "Login successful (JWT coming soon)"});
        }
    }
}