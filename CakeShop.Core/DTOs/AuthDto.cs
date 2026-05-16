using System.ComponentModel.DataAnnotations;

namespace CakeShop.Core.DTOs;

public class RegisterRequest
{
    [Required(ErrorMessage = "帳號為必填")]
    [MaxLength(50)]
    [RegularExpression(@"^[a-zA-Z0-9_\-]+$", ErrorMessage = "帳號只允許英數字、底線、連字號")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email 為必填")]
    [EmailAddress(ErrorMessage = "Email 格式不正確")]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "密碼為必填")]
    [MinLength(6, ErrorMessage = "密碼至少 6 個字元")]
    [MaxLength(100)]
    public string Password { get; set; } = string.Empty;
}

public class ForgotPasswordRequest
{
    [Required(ErrorMessage = "Email 為必填")]
    [EmailAddress(ErrorMessage = "Email 格式不正確")]
    public string Email { get; set; } = string.Empty;
}

public class ResetPasswordRequest
{
    [Required]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "密碼為必填")]
    [MinLength(6, ErrorMessage = "密碼至少 6 個字元")]
    [MaxLength(100)]
    public string NewPassword { get; set; } = string.Empty;
}
