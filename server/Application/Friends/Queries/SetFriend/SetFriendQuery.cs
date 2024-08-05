using Application.DTO;
using MediatR;

namespace Application.Friends.Queries.SetFriend;

public record SetFriendQuery(string UserId, string FriendId) : IRequest<bool>;