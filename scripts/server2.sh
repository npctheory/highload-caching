dotnet add server/Api/ package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add server/Infrastructure/ package Microsoft.Extensions.Options
dotnet add server/Infrastructure/ package System.IdentityModel.Tokens.Jwt

mkdir -p server/Infrastructure/Generators
mkdir -p server/Infrastructure/Providers
mkdir -p server/Application/Users/Queries/Login


touch server/Application/Interfaces/IDateTimeProvider.cs
touch server/Application/Interfaces/IJwtTokenGenerator.cs
touch server/Infrastructure/Providers/DateTimeProvider.cs
touch server/Infrastructure/Generators/JwtTokenGenerator.cs
touch server/Infrastructure/Common/JwtSettings.cs
touch server/Domain/Entities/Token.cs
touch server/Application/DAO/TokenDAO.cs
touch server/Application/DTO/TokenDTO.cs
touch server/Application/Interfaces/ITokenRepository.cs
touch server/Infrastructure/Repositories/TokenRepository.cs
touch server/Application/Users/Queries/Login/LoginQuery.cs
touch server/Application/Users/Queries/Login/LoginQueryHandler.cs