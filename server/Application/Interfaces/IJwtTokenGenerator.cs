namespace Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(string user_id,string first_name,string second_name);
}