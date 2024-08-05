using Application.DTO;
using MediatR;

namespace Application.Users.Queries.SearchUsers;

public record SearchUsersQuery(string first_name, string second_name) : IRequest<List<UserDTO>>;