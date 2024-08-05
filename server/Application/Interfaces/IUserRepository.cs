using Application.DAO;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<UserDAO> GetUserByIdAsync(string userId);
    Task<List<UserDAO>> SearchUsersAsync(string firstName, string lastName);
    Task AddUserAsync(UserDAO user);
}