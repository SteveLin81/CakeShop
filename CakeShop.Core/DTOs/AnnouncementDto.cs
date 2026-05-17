namespace CakeShop.Core.DTOs;

public class AnnouncementDto
{
    public int    Id      { get; set; }
    public string Content { get; set; } = string.Empty;
    public string ContentEn { get; set; } = string.Empty;
    public string ContentJa { get; set; } = string.Empty;
    public string ContentZhCn { get; set; } = string.Empty;
    public string ContentTh { get; set; } = string.Empty;
    public string ContentKo { get; set; } = string.Empty;
    public string ContentVi { get; set; } = string.Empty;
    public string ContentMs { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
