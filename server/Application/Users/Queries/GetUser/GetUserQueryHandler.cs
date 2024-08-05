using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.DAO;
using Application.Users.Queries.GetUser;
using Domain.Entities;
using Mapster;
using MediatR;
using Application.DTO;


namespace Application.Users.Queries.GetUser;

public class GetUserQueryHandler  : IRequestHandler<GetUserQuery, UserDTO>
{
    private readonly IUserRepository _usersRepository;


    public GetUserQueryHandler(IUserRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<UserDTO> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        User user = (await _usersRepository.GetUserByIdAsync(request.Id)).Adapt<User>();
        return user.Adapt<UserDTO>();
    }
}