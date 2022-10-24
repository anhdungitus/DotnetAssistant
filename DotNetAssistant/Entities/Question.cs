using DotNetAssistant.Data;

namespace DotNetAssistant.Entities;

public class Question : BaseEntity
{
    public string? Text { get; set; }
    public DateTime CreatedOnUtc { get; set; }
}