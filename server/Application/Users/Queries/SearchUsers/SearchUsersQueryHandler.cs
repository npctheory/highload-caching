using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.DAO;
using Application.Users.Queries.SearchUsers;
using Domain.Entities;
using AutoMapper;
using MediatR;
using Application.DTO;

namespace Application.Users.Queries.SearchUsers
{
    public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, List<UserDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public SearchUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserDTO>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
        {
            List<UserDAO> userDAOs = await _userRepository.SearchUsersAsync(request.first_name, request.second_name);
            List<User> users = _mapper.Map<List<User>>(userDAOs);
            return _mapper.Map<List<UserDTO>>(users);
        }
    }
}