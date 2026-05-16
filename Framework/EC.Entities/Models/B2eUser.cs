namespace EC.Entities.Models;

public class B2eUser : AuditableEntity
{
    public int      Id                 { get; set; }
    public string   Username           { get; set; } = string.Empty;
    public string   PasswordHash       { get; set; } = string.Empty;
    public string   Email              { get; set; } = string.Empty;
    public int?     RoleId             { get; set; }
    public bool      MustChangePassword { get; set; } = false;
    public string?   ResetToken         { get; set; }
    public DateTime? ResetTokenExpires  { get; set; }
    public B2eRole?  Role               { get; set; }
}
