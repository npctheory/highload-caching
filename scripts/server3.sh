mkdir -p server/Api/Middleware
mkdir -p server/Application/Commands/CreatePost
mkdir -p server/Application/Commands/RegisterUser
mkdir -p server/Application/Commands/UpdatePost
mkdir -p server/Application/Commands/DeletePost
mkdir -p server/Application/Commands/SetFriend
mkdir -p server/Application/Queries/GetPost
mkdir -p server/Application/Services
mkdir -p server/Application/Validators
mkdir -p server/Domain/ValueObjects




touch server/Application/Commands/RegisterUser/RegisterUserCommand.cs
touch server/Application/Commands/RegisterUser/RegisterUserCommandHandler.cs
touch server/Application/Commands/RegisterUser/RegisterUserCommandValidator.cs
touch server/Application/Validators/RegisterUserCommandValidator.cs
touch server/Api/Middleware/JwtMiddleware.cs
touch server/Application/Interfaces/IJwtTokenGenerator.cs
touch server/Application/Services/JwtTokenGenerator.cs

touch server/Application/Commands/SetFriend/SetFriendCommand.cs
touch server/Application/Commands/SetFriend/SetFriendCommandHandler.cs

touch server/Api/Controllers/PostController.cs
touch server/Application/Commands/CreatePost/CreatePostCommand.cs
touch server/Application/Commands/CreatePost/CreatePostCommandHandler.cs
touch server/Application/Commands/CreatePost/CreatePostCommandValidator.cs
touch server/Application/Commands/UpdatePost/UpdatePostCommand.cs
touch server/Application/Commands/UpdatePost/UpdatePostCommandHandler.cs
touch server/Application/Commands/UpdatePost/UpdatePostCommandValidator.cs
touch server/Application/Commands/DeletePost/DeletePostCommand.cs
touch server/Application/Commands/DeletePost/DeletePostCommandHandler.cs
touch server/Application/Queries/GetPost/GetPostQuery.cs
touch server/Application/Queries/GetPost/GetPostQueryHandler.cs
touch server/Application/DTOs/PostDto.cs
touch server/Application/Interfaces/IPostRepository.cs
touch server/Infrastructure/Repositories/PostRepository.cs
touch server/Application/Validators/CreatePostCommandValidator.cs
touch server/Domain/Entities/Post.cs

touch server/Infrastructure/Configurations/ServiceCollectionExtensions.cs

touch server/Api/Controllers/DialogController.cs
touch server/Application/DTOs/DialogMessageDto.cs
touch server/Domain/Entities/DialogMessage.cs

dotnet add server/Api/ package Microsoft.Extensions.DependencyInjection.Abstractions
dotnet add server/Api/ package System.IdentityModel.Tokens.Jwt
dotnet add server/Api/ package Microsoft.Extensions.Options.ConfigurationExtensions

dotnet add server/Api/ package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add server/Api/ package StackExchange.Redis
dotnet add server/Api/ package RabbitMQ.Client
dotnet add server/Api/ package MassTransit
