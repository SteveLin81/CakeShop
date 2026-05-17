namespace EC.Entities.Models;

public class SystemLog
{
    public int      Id           { get; set; }
    public DateTime LogTime      { get; set; } = DateTime.UtcNow;
    public string   Username     { get; set; } = string.Empty;
    public string   Project      { get; set; } = string.Empty;
    public string   FunctionName { get; set; } = string.Empty;
    public string   ErrorMessage { get; set; } = string.Empty;
    public string?  ExceptionMsg { get; set; }
    public string   LogLevel     { get; set; } = "Error";
}
