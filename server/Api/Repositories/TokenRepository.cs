using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using Api.Models;
using Npgsql;

namespace Api.Repositories;

public interface ITokensRepository
{
    Task AddTokenAsync(Token token);
    Task<Token> GetTokenByValueAsync(string tokenValue);
    Task<List<Token>> GetTokensByUserIdAsync(string userId);
}

public class TokensRepository : ITokensRepository
{
    private readonly string _connectionString;

    public TokensRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public async Task AddTokenAsync(Token token)
    {
        if (token == null) throw new ArgumentNullException(nameof(token));

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new NpgsqlCommand("INSERT INTO tokens (user_id, token, expires_at) VALUES (@userId, @token, @expiresAt)", connection))
            {
                command.Parameters.AddWithValue("userId", token.UserId);
                command.Parameters.AddWithValue("token", token.Value);
                command.Parameters.AddWithValue("expiresAt", token.ExpiresAt);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<Token> GetTokenByValueAsync(string tokenValue)
    {
        if (string.IsNullOrEmpty(tokenValue)) throw new ArgumentException("Token value cannot be null or empty.", nameof(tokenValue));

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new NpgsqlCommand("SELECT id, user_id, token, created_at, expires_at FROM tokens WHERE token = @token", connection))
            {
                command.Parameters.AddWithValue("token", tokenValue);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Token
                        {
                            Id = reader.GetGuid(0),
                            UserId = reader.GetString(1),
                            Value = reader.GetString(2),
                            CreatedAt = reader.GetDateTime(3),
                            ExpiresAt = reader.GetDateTime(4)
                        };
                    }
                    return null;
                }
            }
        }
    }

    public async Task<List<Token>> GetTokensByUserIdAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId)) throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

        var tokens = new List<Token>();

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new NpgsqlCommand("SELECT id, user_id, token, created_at, expires_at FROM tokens WHERE user_id = @userId", connection))
            {
                command.Parameters.AddWithValue("userId", userId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tokens.Add(new Token
                        {
                            Id = reader.GetGuid(0),
                            UserId = reader.GetString(1),
                            Value = reader.GetString(2),
                            CreatedAt = reader.GetDateTime(3),
                            ExpiresAt = reader.GetDateTime(4)
                        });
                    }
                }
            }
        }

        return tokens;
    }
}
