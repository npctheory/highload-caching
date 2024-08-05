using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Token
{
    public Guid Id {get; set;}
    public string UserId { get; set; }
    public string Value { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}