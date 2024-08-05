using Application.DAO;
using Application.Interfaces;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FriendRepository : IFriendRepository
{
    private readonly string _connectionString;

    public FriendRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task AddAsync(string userId, string friendId)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand("INSERT INTO friends (user_id, friend_id) VALUES (@UserId, @FriendId)", connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@FriendId", friendId);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task DeleteAsync(string userId, string friendId)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand("DELETE FROM friends WHERE user_id = @UserId AND friend_id = @FriendId", connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@FriendId", friendId);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<List<FriendDAO>> ListAsync(string userId)
    {
        var friends = new List<FriendDAO>();

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new NpgsqlCommand("SELECT friend_id FROM friends WHERE user_id = @UserId", connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        friends.Add(new FriendDAO
                        {
                            UserId = userId,
                            FriendId = reader.GetString(0) // Assuming friend_id is a string
                        });
                    }
                }
            }
        }

        return friends;
    }
}
