using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.DAO;
using Application.Users.Queries.GetUser;
using Domain.Entities;
using AutoMapper;
using MediatR;
using Application.DTO;

namespace Application.Users.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDTO>
    {
        private readonly IUserRepository _usersRepository;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IUserRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            UserDAO userDAO = await _usersRepository.GetUserByIdAsync(request.Id);
            User user = _mapper.Map<User>(userDAO);
            return _mapper.Map<UserDTO>(user);
        }
    }
}
