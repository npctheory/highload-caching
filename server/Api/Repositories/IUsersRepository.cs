using Api.Models;
using Api.Services.Authentication;

namespace Api.Repositories;

public interface IUsersRepository
{
    Task<User> GetUserByIdAsync(string userId);
    Task<List<User>> SearchUsersAsync(string firstName, string lastName);
    Task AddUserAsync(User user);
}