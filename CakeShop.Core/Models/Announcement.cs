namespace CakeShop.Core.Models;

public class Announcement
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string ContentEn { get; set; } = string.Empty;
    public string ContentJa { get; set; } = string.Empty;
    public string ContentZhCn { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
