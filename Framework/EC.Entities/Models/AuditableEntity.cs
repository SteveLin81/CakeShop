namespace EC.Entities.Models;

public abstract class AuditableEntity
{
    public DateTime CreatedAt   { get; set; }
    public string   CreatedBy   { get; set; } = "admin";
    public DateTime UpdatedAt   { get; set; }
    public string   UpdatedBy   { get; set; } = "admin";
    public int      UpdateCount { get; set; }
}
