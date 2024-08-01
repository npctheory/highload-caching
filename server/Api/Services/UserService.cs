using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DTO;
using Api.Models;
using Api.Repositories;


namespace Api.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(string userId);
        Task<List<User>> SearchUsersAsync(string firstName, string lastName);
    }

    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;


        public UserService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            return await _usersRepository.GetUserByIdAsync(userId);
        }

        public async Task<List<User>> SearchUsersAsync(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName))
                throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));

            if (string.IsNullOrEmpty(lastName))
                throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));


            return await _usersRepository.SearchUsersAsync(firstName, lastName);
        }
    }
}
