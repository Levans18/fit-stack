using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using FitStack.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FitStack.API.Data;

namespace FitStack.API.Services
{
    public class CurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public CurrentUserService(IHttpContextAccessor accessor, AppDbContext context)
        {
            _httpContextAccessor = accessor;
            _context = context;
        }

        public async Task<(User? user, string? error)> GetAsync()
        {
            var userPrincipal = _httpContextAccessor.HttpContext?.User;
            if (userPrincipal == null)
                return (null, "Missing authentication context.");

            var sub = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (sub == null)
                return (null, "Token is missing user ID.");
            if (!int.TryParse(sub, out var userId))
                return (null, "Invalid user ID format in token.");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return (null, "User not found.");

            return (user, null);
        }
    }
}