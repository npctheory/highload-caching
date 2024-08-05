namespace Infrastructure.Common;

using Npgsql;
using System;

public class PostgresConnectionFactory
{
    private readonly string _connectionString;

    public PostgresConnectionFactory()
    {
        _connectionString = BuildConnectionString();
    }

    private string BuildConnectionString()
    {
        var host = Environment.GetEnvironmentVariable("PRIMARY_DB_HOST") ?? throw new InvalidOperationException("PRIMARY_DB_HOST environment variable is not set.");
        var port = Environment.GetEnvironmentVariable("PRIMARY_DB_PORT") ?? throw new InvalidOperationException("PRIMARY_DB_PORT environment variable is not set.");
        var database = Environment.GetEnvironmentVariable("PRIMARY_DB_NAME") ?? throw new InvalidOperationException("PRIMARY_DB_NAME environment variable is not set.");
        var username = Environment.GetEnvironmentVariable("PRIMARY_DB_USER") ?? throw new InvalidOperationException("PRIMARY_DB_USER environment variable is not set.");
        var password = Environment.GetEnvironmentVariable("PRIMARY_DB_PASSWORD") ?? throw new InvalidOperationException("PRIMARY_DB_PASSWORD environment variable is not set.");

        return $"Host={host};Port={port};Database={database};Username={username};Password={password};";
    }

    public NpgsqlConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}