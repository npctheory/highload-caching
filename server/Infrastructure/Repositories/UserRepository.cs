using System.Net.Sockets;
using System.Net.Mime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Bogus;
using Npgsql;
using Application.Interfaces;
using Application.DAO;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public async Task<UserDAO> GetUserByIdAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId)) throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new NpgsqlCommand("SELECT id, password_hash, first_name, second_name, birthdate, biography, city FROM users WHERE id = @id", connection))
            {
                command.Parameters.AddWithValue("id", userId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new UserDAO
                        {
                            Id = reader.GetString(0),
                            PasswordHash = reader.GetString(1),
                            FirstName = reader.GetString(2),
                            SecondName = reader.GetString(3),
                            Birthdate = reader.GetDateTime(4),
                            Biography = reader.IsDBNull(5) ? null : reader.GetString(5),
                            City = reader.IsDBNull(6) ? null : reader.GetString(6)
                        };
                    }
                    return null;
                }
            }
        }
    }

    public async Task<List<UserDAO>> SearchUsersAsync(string firstName, string lastName)
    {
        if (string.IsNullOrEmpty(firstName)) throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));
        if (string.IsNullOrEmpty(lastName)) throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));

        var users = new List<UserDAO>();

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            var query = @"
                SELECT id, password_hash, first_name, second_name, birthdate, biography, city 
                FROM users 
                WHERE first_name ILIKE @firstName AND second_name ILIKE @lastName";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("firstName", $"{firstName}%");
                command.Parameters.AddWithValue("lastName", $"{lastName}%");

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        users.Add(new UserDAO
                        {
                            Id = reader.GetString(0),
                            PasswordHash = reader.GetString(1),
                            FirstName = reader.GetString(2),
                            SecondName = reader.GetString(3),
                            Birthdate = reader.GetDateTime(4),
                            Biography = reader.IsDBNull(5) ? null : reader.GetString(5),
                            City = reader.IsDBNull(6) ? null : reader.GetString(6)
                        });
                    }
                }
            }
        }

        return users;
    }

    public async Task AddUserAsync(UserDAO user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        var faker = new Faker();
        var randomUsername = faker.Random.Word() + faker.Random.Word() + faker.Random.Number(1000, 9999);

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new NpgsqlCommand("INSERT INTO users (id, password_hash, first_name, second_name, birthdate, biography, city) VALUES (@id, @passwordHash, @firstName, @secondName, @birthdate, @biography, @city)", connection))
            {
                command.Parameters.AddWithValue("id", randomUsername);
                command.Parameters.AddWithValue("passwordHash", user.PasswordHash);
                command.Parameters.AddWithValue("firstName", user.FirstName);
                command.Parameters.AddWithValue("secondName", user.SecondName);
                command.Parameters.AddWithValue("birthdate", user.Birthdate);
                command.Parameters.AddWithValue("biography", user.Biography ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("city", user.City ?? (object)DBNull.Value);

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}