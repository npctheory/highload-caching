using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Api.Repositories;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Api.Services
{
    public interface IFriendsService
    {
        Task<IEnumerable<string>> GetFriendsForCurrentUserAsync(HttpContext httpContext);
    }

    public class FriendsService : IFriendsService
    {
        private readonly IFriendsRepository _friendsRepository;
        private readonly ILogger<FriendsService> _logger;

        public FriendsService(IFriendsRepository friendsRepository, ILogger<FriendsService> logger)
        {
            _friendsRepository = friendsRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> GetFriendsForCurrentUserAsync(HttpContext httpContext)
        {
            // Log all claims for debugging
            var claims = httpContext.User.Claims;
            foreach (var claim in claims)
            {
                _logger.LogInformation("Claim Type: {Type}, Value: {Value}", claim.Type, claim.Value);
            }

            // Retrieve the user ID from the JWT claims
            var userId = httpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User ID not found in token.");
                throw new UnauthorizedAccessException("User ID not found in token.");
            }

            _logger.LogInformation("Retrieved User ID: {UserId}", userId);

            return await _friendsRepository.GetFriendsByUserIdAsync(userId);
        }
    }
}
