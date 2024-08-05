using Application.DTO;
using MediatR;

namespace Application.Friends.Queries.ListFriends;

public record ListFriendsQuery(string userId) : IRequest<List<FriendDTO>>;