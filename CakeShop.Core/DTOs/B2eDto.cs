using System.ComponentModel.DataAnnotations;

namespace CakeShop.Core.DTOs;

// ── B2C 帳號管理（後台管理 B2C 用戶）─────────────────────────────────
public class B2cUserDto
{
    public int      Id          { get; set; }
    public string   Username    { get; set; } = string.Empty;
    public string   Email       { get; set; } = string.Empty;
    public DateTime CreatedAt   { get; set; }
    public DateTime UpdatedAt   { get; set; }
    public int      UpdateCount { get; set; }
}

public class B2cUserCreateRequest
{
    [Required(ErrorMessage = "帳號為必填")]
    [MaxLength(50, ErrorMessage = "帳號不得超過 50 個字元")]
    [RegularExpression(@"^[a-zA-Z0-9_\-]+$", ErrorMessage = "帳號只允許英數字、底線、連字號")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "密碼為必填")]
    [MinLength(6, ErrorMessage = "密碼至少 6 個字元")]
    [MaxLength(100, ErrorMessage = "密碼不得超過 100 個字元")]
    public string Password { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Email 格式不正確")]
    [MaxLength(100, ErrorMessage = "Email 不得超過 100 個字元")]
    public string Email { get; set; } = string.Empty;
}

public class B2cUserUpdateRequest
{
    [EmailAddress(ErrorMessage = "Email 格式不正確")]
    [MaxLength(100, ErrorMessage = "Email 不得超過 100 個字元")]
    public string  Email       { get; set; } = string.Empty;

    [MinLength(6, ErrorMessage = "密碼至少 6 個字元")]
    [MaxLength(100, ErrorMessage = "密碼不得超過 100 個字元")]
    public string? NewPassword { get; set; }
}

// ── 商品管理 ───────────────────────────────────────────────────────────
public class ProductSaveRequest
{
    [MaxLength(100)] public string  Name            { get; set; } = string.Empty;
    [MaxLength(100)] public string  NameEn          { get; set; } = string.Empty;
    [MaxLength(100)] public string  NameJa          { get; set; } = string.Empty;
    [MaxLength(100)] public string  NameZhCn        { get; set; } = string.Empty;
    [MaxLength(100)] public string  NameTh          { get; set; } = string.Empty;
    [MaxLength(100)] public string  NameKo          { get; set; } = string.Empty;
    [MaxLength(100)] public string  NameVi          { get; set; } = string.Empty;
    [MaxLength(100)] public string  NameMs          { get; set; } = string.Empty;
    public string  Description     { get; set; } = string.Empty;
    public string  DescriptionEn   { get; set; } = string.Empty;
    public string  DescriptionJa   { get; set; } = string.Empty;
    public string  DescriptionZhCn { get; set; } = string.Empty;
    public string  DescriptionTh   { get; set; } = string.Empty;
    public string  DescriptionKo   { get; set; } = string.Empty;
    public string  DescriptionVi   { get; set; } = string.Empty;
    public string  DescriptionMs   { get; set; } = string.Empty;

    [Range(0, 999999, ErrorMessage = "售價必須在 0~999999 之間")]
    public decimal Price           { get; set; }

    [MaxLength(500)] public string  ImageUrl  { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "請選擇有效分類")]
    public int     CategoryId      { get; set; }
    public bool    IsAvailable     { get; set; } = true;
    public bool    IsFeatured      { get; set; } = false;
}

// ── 分類管理 ───────────────────────────────────────────────────────────
public class CategorySaveRequest
{
    [Required(ErrorMessage = "分類名稱為必填")]
    [MaxLength(50)] public string Name    { get; set; } = string.Empty;
    [MaxLength(50)] public string NameEn  { get; set; } = string.Empty;
    [MaxLength(50)] public string NameJa  { get; set; } = string.Empty;
    [MaxLength(50)] public string NameZhCn{ get; set; } = string.Empty;
    [MaxLength(50)] public string NameTh  { get; set; } = string.Empty;
    [MaxLength(50)] public string NameKo  { get; set; } = string.Empty;
    [MaxLength(50)] public string NameVi  { get; set; } = string.Empty;
    [MaxLength(50)] public string NameMs  { get; set; } = string.Empty;
}

// ── 公告管理 ───────────────────────────────────────────────────────────
public class AnnouncementSaveRequest
{
    public string Content       { get; set; } = string.Empty;
    public string ContentEn     { get; set; } = string.Empty;
    public string ContentJa     { get; set; } = string.Empty;
    public string ContentZhCn   { get; set; } = string.Empty;
    public string ContentTh     { get; set; } = string.Empty;
    public string ContentKo     { get; set; } = string.Empty;
    public string ContentVi     { get; set; } = string.Empty;
    public string ContentMs     { get; set; } = string.Empty;
    public bool   IsActive      { get; set; }
}

// ── 通用 API 回應包裝 ───────────────────────────────────────────────────
public class ApiResult<T>
{
    public bool   Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T?     Data    { get; set; }

    public static ApiResult<T> Ok(T data, string msg = "")
        => new() { Success = true, Data = data, Message = msg };
    public static ApiResult<T> Fail(string msg)
        => new() { Success = false, Message = msg };
}
