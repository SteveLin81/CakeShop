namespace EC.Entities.Models;

public class User : AuditableEntity
{
    public int       Id                 { get; set; }
    public string    Username           { get; set; } = string.Empty;
    public string    PasswordHash       { get; set; } = string.Empty;
    public string    Email              { get; set; } = string.Empty;
    public string?   ResetToken         { get; set; }
    public DateTime? ResetTokenExpires  { get; set; }
}
