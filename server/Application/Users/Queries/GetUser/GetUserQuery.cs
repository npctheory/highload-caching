using Application.DTO;
using MediatR;

namespace Application.Users.Queries.GetUser;

public record GetUserQuery(string Id) : IRequest<UserDTO>;