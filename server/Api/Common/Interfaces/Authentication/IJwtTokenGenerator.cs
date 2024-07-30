namespace Api.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(string user_id,string first_name,string second_name);
}
