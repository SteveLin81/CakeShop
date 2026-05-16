namespace CakeShop.Core.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string NameJa { get; set; } = string.Empty;
    public string NameZhCn { get; set; } = string.Empty;
    public string NameTh { get; set; } = string.Empty;
    public string NameKo { get; set; } = string.Empty;
    public string NameVi { get; set; } = string.Empty;
    public string NameMs { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string DescriptionJa { get; set; } = string.Empty;
    public string DescriptionZhCn { get; set; } = string.Empty;
    public string DescriptionTh { get; set; } = string.Empty;
    public string DescriptionKo { get; set; } = string.Empty;
    public string DescriptionVi { get; set; } = string.Empty;
    public string DescriptionMs { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryNameEn { get; set; } = string.Empty;
    public string CategoryNameJa { get; set; } = string.Empty;
    public string CategoryNameZhCn { get; set; } = string.Empty;
    public string CategoryNameTh { get; set; } = string.Empty;
    public string CategoryNameKo { get; set; } = string.Empty;
    public string CategoryNameVi { get; set; } = string.Empty;
    public string CategoryNameMs { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public bool IsFeatured  { get; set; }
}
