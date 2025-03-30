
using FitStack.API.DTOs;
using FitStack.API.Data;
using FitStack.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using FitStack.API.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace FitStack.API.Controllers
{

    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;
        private readonly PasswordHasher<User> _hasher = new();

        public AuthController(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
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

            var token = _tokenService.CreateToken(user);
    
            return Ok(new
            {
                message = "User Registered Successfully",
                token,
                user = new
                {
                    id = user.Id,
                    username = user.Username,
                    email = user.Email,
                }
            });
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

            var token = _tokenService.CreateToken(user);

            return Ok(new
            {
                message = "Login Successful",
                token,
                user = new
                {
                    id = user.Id,
                    username = user.Username,
                    email = user.Email,
                }
            });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userIdClaim == null)
                return Unauthorized("Invalid Token");

            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid user ID");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User Not Found.");

            return Ok(new
            {
                message = "Authenticated User Retreived",
                user = new
                {
                    id = user.Id,
                    username = user.Username,
                    email = user.Email
                }
            });
        }
    }
}