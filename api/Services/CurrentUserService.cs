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

        public (int? userId, string? error) GetUserIdFromToken()
        {
            var userPrincipal = _httpContextAccessor.HttpContext?.User;
            if (userPrincipal == null)
                return (null, "Missing authentication context.");

            var sub = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

            if (sub == null)
                return (null, "Token is missing user ID.");
            if (!int.TryParse(sub, out var userId))
                return (null, "Invalid user ID format in token.");

            return (userId, null);
        }

        public async Task<(User? user, string? error)> GetAsync()
        {
            // Check if the user is already cached in HttpContext.Items
            if (_httpContextAccessor.HttpContext?.Items["CurrentUser"] is User cachedUser)
                return (cachedUser, null);

            // Validate the token and extract the user ID
            var (userId, tokenError) = GetUserIdFromToken();
            if (userId == null)
                return (null, tokenError);

            // Query the database only if necessary
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return (null, "User not found.");

            // Cache the user in HttpContext.Items
            if (_httpContextAccessor.HttpContext?.Items != null)
                _httpContextAccessor.HttpContext.Items["CurrentUser"] = user;

            return (user, null);
        }
    }
}