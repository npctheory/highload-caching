using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.DAO;
using Application.Users.Queries.GetUser;
using Domain.Entities;
using AutoMapper;
using MediatR;
using Application.DTO;

namespace Application.Friends.Queries.DeleteFriend
{
    public class DeleteFriendQueryHandler : IRequestHandler<DeleteFriendQuery, bool>
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IMapper _mapper;

        public DeleteFriendQueryHandler(IFriendRepository friendRepository, IMapper mapper)
        {
            _friendRepository = friendRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteFriendQuery request, CancellationToken cancellationToken)
        {
            await _friendRepository.DeleteAsync(request.UserId, request.FriendId);
            return true;
        }
    }
}