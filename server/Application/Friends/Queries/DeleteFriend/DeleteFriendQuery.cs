using Application.DTO;
using MediatR;

namespace Application.Friends.Queries.DeleteFriend;

public record DeleteFriendQuery(string UserId, string FriendId) : IRequest<bool>;