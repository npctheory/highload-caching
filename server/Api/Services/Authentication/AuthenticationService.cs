using System;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Api.Models;
using Api.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Api.Authentication;

namespace Api.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> LoginAsync(string userId, string password);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ITokensRepository _tokensRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthenticationService(IUsersRepository usersRepository, ITokensRepository tokensRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            _tokensRepository = tokensRepository ?? throw new ArgumentNullException(nameof(tokensRepository));
            _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
        }

        public async Task<AuthenticationResult> LoginAsync(string userId, string password)
        {
            var user = await _usersRepository.GetUserByIdAsync(userId);

            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid user ID or password.");
            }

            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.FirstName, user.SecondName);

            // Optionally store the token in the repository
            var tokenEntity = new Token
            {
                UserId = user.Id,
                Value = token,
                ExpiresAt = DateTime.UtcNow.AddHours(1) // Example expiration time
            };

            await _tokensRepository.AddTokenAsync(tokenEntity);

            return new AuthenticationResult
            {
                Token = token
            };
        }

        private bool VerifyPassword(string password, string storedPasswordHash)
        {
            var parts = storedPasswordHash.Split(':');
            if (parts.Length != 2) return false;

            var salt = parts[0];
            var hash = parts[1];

            using (var sha256 = SHA256.Create())
            {
                var computedHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                var computedHashString = BitConverter.ToString(computedHash).Replace("-", "").ToLower();

                return hash == computedHashString;
            }
        }
    }
}
