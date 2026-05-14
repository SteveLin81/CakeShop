namespace EC.Entities.Models;

public class Product
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
    public string Description { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string DescriptionJa { get; set; } = string.Empty;
    public string DescriptionZhCn { get; set; } = string.Empty;
    public string? DescriptionTh { get; set; }
    public string? DescriptionKo { get; set; }
    public string? DescriptionVi { get; set; }
    public string? DescriptionMs { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}
