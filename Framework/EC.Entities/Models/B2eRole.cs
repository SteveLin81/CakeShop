namespace EC.Entities.Models;

public class B2eRole : AuditableEntity
{
    public int    Id          { get; set; }
    public string Name        { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Permissions { get; set; } = "[]"; // JSON array of permission keys
    public string NameEn      { get; set; } = string.Empty;
    public string NameJa      { get; set; } = string.Empty;
    public string NameZhCn    { get; set; } = string.Empty;
    public string NameTh      { get; set; } = string.Empty;
    public string NameKo      { get; set; } = string.Empty;
    public string NameVi      { get; set; } = string.Empty;
    public string NameMs      { get; set; } = string.Empty;
    public string DescriptionEn   { get; set; } = string.Empty;
    public string DescriptionJa   { get; set; } = string.Empty;
    public string DescriptionZhCn { get; set; } = string.Empty;
    public string DescriptionTh   { get; set; } = string.Empty;
    public string DescriptionKo   { get; set; } = string.Empty;
    public string DescriptionVi   { get; set; } = string.Empty;
    public string DescriptionMs   { get; set; } = string.Empty;
}
