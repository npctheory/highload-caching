using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.DAO;
using Application.Users.Queries.GetUser;
using Domain.Entities;
using AutoMapper;
using MediatR;
using Application.DTO;

namespace Application.Friends.Queries.SetFriend
{
    public class SetFriendQueryHandler : IRequestHandler<SetFriendQuery, bool>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IMapper _mapper;

        public SetFriendQueryHandler(IFriendRepository friendRepository, IMapper mapper)
        {
            _friendRepository = friendRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(SetFriendQuery request, CancellationToken cancellationToken)
        {
            await _friendRepository.AddAsync(request.UserId, request.FriendId);
            return true;
        }
    }
}