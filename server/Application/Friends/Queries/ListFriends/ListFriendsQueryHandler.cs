using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.DAO;
using Application.Users.Queries.SearchUsers;
using Domain.Entities;
using AutoMapper;
using MediatR;
using Application.DTO;

namespace Application.Friends.Queries.ListFriends
{
    public class ListFriendsQueryHandler : IRequestHandler<ListFriendsQuery, List<FriendDTO>>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IMapper _mapper;

        public ListFriendsQueryHandler(IFriendRepository friendRepository, IMapper mapper)
        {
            _friendRepository = friendRepository;
            _mapper = mapper;
        }

        public async Task<List<FriendDTO>> Handle(ListFriendsQuery request, CancellationToken cancellationToken)
        {
            List<FriendDAO> friendDAOs = await _friendRepository.ListAsync(request.userId);

            List<Friend> friends = _mapper.Map<List<Friend>>(friendDAOs);
            return _mapper.Map<List<FriendDTO>>(friends);
        }
    }
}