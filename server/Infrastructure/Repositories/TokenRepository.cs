using System.Net.Security;
using System.Net.Mime;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using Application.Interfaces;
using Application.DAO;
using Npgsql;

namespace Infrastructure.Repositories;


public class TokenRepository : ITokenRepository
{
    private readonly string _connectionString;

    public TokenRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public async Task AddTokenAsync(TokenDAO token)
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

    public async Task<TokenDAO> GetTokenByValueAsync(string tokenValue)
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
                        return new TokenDAO
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

    public async Task<List<TokenDAO>> GetTokensByUserIdAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId)) throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

        var tokens = new List<TokenDAO>();

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
                        tokens.Add(new TokenDAO
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