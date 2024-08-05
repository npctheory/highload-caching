using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DAO;

namespace Application.Interfaces;

public interface IFriendRepository
{
    Task AddAsync(string userId, string friendId);
    Task DeleteAsync(string userId, string friendId);
    Task<List<FriendDAO>> ListAsync(string userId);
}
