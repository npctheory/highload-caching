using System;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Threading;
using Application.Interfaces;
using Application.DTO;
using Domain.Entities;
using Application.DAO;
using AutoMapper;
using MediatR;
using System.Security.Claims;

namespace Application.Users.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, TokenDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public LoginQueryHandler(
            IUserRepository userRepository,
            ITokenRepository tokenRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
            _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TokenDTO> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var userDAO = await _userRepository.GetUserByIdAsync(request.Id);

            if (userDAO == null || !VerifyPassword(request.Password, userDAO.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid user ID or password.");
            }

            User user = _mapper.Map<User>(userDAO);

            Token token = new Token
            {
                UserId = user.Id,
                Value = _jwtTokenGenerator.GenerateToken(user.Id, user.FirstName, user.SecondName),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddYears(2)
            };

            await _tokenRepository.AddTokenAsync(_mapper.Map<TokenDAO>(token));

            return _mapper.Map<TokenDTO>(token);
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