namespace EC.Entities.Models;

public class Category : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string NameJa { get; set; } = string.Empty;
    public string NameZhCn { get; set; } = string.Empty;
    public string? NameTh { get; set; }
    public string? NameKo { get; set; }
    public string? NameVi { get; set; }
    public string? NameMs { get; set; }
}
