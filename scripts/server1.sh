dotnet new sln -o HighloadSocial
mv HighloadSocial server

dotnet new webapi -o server/Api
dotnet new classlib -o server/Application
dotnet new classlib -o server/Infrastructure
dotnet new classlib -o server/Domain

dotnet sln server/HighloadSocial.sln add server/Api/
dotnet sln server/HighloadSocial.sln add server/Application/
dotnet sln server/HighloadSocial.sln add server/Infrastructure/
dotnet sln server/HighloadSocial.sln add server/Domain/

dotnet add server/Api/Api.csproj reference server/Application/Application.csproj
dotnet add server/Api/Api.csproj reference server/Infrastructure/Infrastructure.csproj
dotnet add server/Infrastructure/Infrastructure.csproj reference server/Application/
dotnet add server/Application/Application.csproj reference server/Domain/Domain.csproj

rm server/Application/Class1.cs
rm server/Infrastructure/Class1.cs
rm server/Domain/Class1.cs

mkdir -p server/Api/Controllers
mkdir -p server/Application/Users/Queries/GetUser
mkdir -p server/Application/Users/Queries/SearchUsers
mkdir -p server/Application/Interfaces
mkdir -p server/Application/DTO
mkdir -p server/Application/DAO
mkdir -p server/Application/Mapping
mkdir -p server/Infrastructure/Common
mkdir -p server/Infrastructure/Repositories
mkdir -p server/Domain/Entities


touch server/Application/DTO/UserDTO.cs
touch server/Domain/Entities/User.cs
touch server/Application/DAO/UserDAO.cs
touch server/Infrastructure/Common/PostgresConnectionFactory.cs
touch server/Application/Mapping/MappingProfile.cs
touch server/Application/Interfaces/IUserRepository.cs
touch server/Infrastructure/Repositories/UserRepository.cs
touch server/Api/Controllers/UserController.cs
touch server/Application/Users/Queries/GetUser/GetUserQuery.cs
touch server/Application/Users/Queries/GetUser/GetUserQueryHandler.cs
touch server/Application/Users/Queries/SearchUsers/SearchUsersQuery.cs
touch server/Application/Users/Queries/SearchUsers/SearchUsersQueryHandler.cs


dotnet add server/Api/ package AutoMapper
dotnet add server/Application/ package AutoMapper
dotnet add server/Application/ package MediatR
dotnet add server/Application/ package MediatR.DependencyInjection
dotnet add server/Infrastructure/ package Npgsql
dotnet add server/Infrastructure/ package Bogus



dotnet add server/Api/ package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add server/Infrastructure/ package Microsoft.Extensions.Options
dotnet add server/Infrastructure/ package System.IdentityModel.Tokens.Jwt