using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Friend
{
    public string UserId { get; set; }
    public string FriendId { get; set; }
}