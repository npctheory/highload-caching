using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace Api.Repositories;

public interface IFriendsRepository
{
    Task<IEnumerable<string>> GetFriendsByUserIdAsync(string userId);
}

public class FriendsRepository : IFriendsRepository
{
    private readonly string _connectionString;

    public FriendsRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<string>> GetFriendsByUserIdAsync(string userId)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            const string query = @"
                SELECT friend_id
                FROM friends
                WHERE user_id = @UserId;
            ";

            return await connection.QueryAsync<string>(query, new { UserId = userId });
        }
    }
}
