namespace EC.Entities.Models;

public class B2eRole : AuditableEntity
{
    public int    Id          { get; set; }
    public string Name        { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Permissions { get; set; } = "[]"; // JSON array of permission keys
}
