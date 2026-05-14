namespace EC.Entities.Models;

public class Announcement
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string ContentEn { get; set; } = string.Empty;
    public string ContentJa { get; set; } = string.Empty;
    public string ContentZhCn { get; set; } = string.Empty;
    public string? ContentTh { get; set; }
    public string? ContentKo { get; set; }
    public string? ContentVi { get; set; }
    public string? ContentMs { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
