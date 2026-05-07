namespace CakeShop.Core.DTOs;

public class ContactFormDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class ContactFormResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
