using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.DAO;
using Application.Users.Queries.GetUser;
using Domain.Entities;
using Mapster;
using MediatR;
using Application.DTO;

namespace Application.Users.Queries.SearchUsers;

public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, List<UserDTO>>
{
    private readonly IUserRepository _userRepository;

    public SearchUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserDTO>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
    {

        List<UserDAO> userDAOs = await _userRepository.SearchUsersAsync(request.first_name, request.second_name);
        List<User> results = userDAOs.Adapt<List<User>>();
        return results.Adapt<List<UserDTO>>();
    }
}