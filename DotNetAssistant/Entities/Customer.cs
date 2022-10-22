using DotNetAssistant.Data;

namespace DotNetAssistant.Entities;

public class Customer : BaseEntity
{
    public string? IdentityId { get; set; }
    public AppUser? Identity { get; set; }
    public string? Location { get; set; }
    public string? Locale { get; set; }
    public string? Gender { get; set; }
}