using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CakeShop.Core.DTOs;

// ── B2E 登入回應（擴充 B2C LoginResponse，加入角色與權限）──────────────
public class B2eLoginResponse
{
    public bool     Success            { get; set; }
    public string   Token              { get; set; } = string.Empty;
    public string   Username           { get; set; } = string.Empty;
    public string   Message            { get; set; } = string.Empty;
    public string   Role               { get; set; } = string.Empty;
    public string[] Permissions        { get; set; } = [];
    public bool     MustChangePassword { get; set; }
}

// ── 修改密碼 ──────────────────────────────────────────────────────────
public class ChangePasswordRequest
{
    [Required(ErrorMessage = "請輸入目前密碼")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "請輸入新密碼")]
    [MinLength(4, ErrorMessage = "新密碼至少 4 個字元")]
    [MaxLength(100)]
    public string NewPassword { get; set; } = string.Empty;
}

// ── 角色管理 ─────────────────────────────────────────────────────────
public class B2eRoleDto
{
    public int      Id          { get; set; }
    public string   Name        { get; set; } = string.Empty;
    public string   Description { get; set; } = string.Empty;
    public string[] Permissions { get; set; } = [];
}

public class B2eRoleSaveRequest
{
    [Required(ErrorMessage = "角色名稱為必填")]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string[] Permissions { get; set; } = [];
}

// ── 後台帳號管理 ──────────────────────────────────────────────────────
public class B2eAdminDto
{
    public int      Id                 { get; set; }
    public string   Username           { get; set; } = string.Empty;
    public string   Email              { get; set; } = string.Empty;
    public int?     RoleId             { get; set; }
    public string   RoleName           { get; set; } = string.Empty;
    public string[] Permissions        { get; set; } = [];
    public bool     MustChangePassword { get; set; }
    public DateTime CreatedAt          { get; set; }
    public DateTime UpdatedAt          { get; set; }
    public int      UpdateCount        { get; set; }
}

public class B2eAdminCreateRequest
{
    [Required(ErrorMessage = "帳號為必填")]
    [MaxLength(50)]
    [RegularExpression(@"^[a-zA-Z0-9_\-]+$", ErrorMessage = "帳號只允許英數字、底線、連字號")]
    public string Username { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Email 格式不正確")]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    public int? RoleId { get; set; }
}

public class B2eAdminUpdateRequest
{
    [EmailAddress(ErrorMessage = "Email 格式不正確")]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    public int? RoleId { get; set; }
}

// ── 可用權限常數 ──────────────────────────────────────────────────────
public static class B2ePermissions
{
    public const string Dashboard     = "dashboard";
    public const string Products      = "products";
    public const string Categories    = "categories";
    public const string Announcements = "announcements";
    public const string Members       = "members";
    public const string Homepage      = "homepage";
    public const string Roles         = "roles";
    public const string Admins        = "admins";

    public static readonly string[] All =
        [Dashboard, Products, Categories, Announcements, Members, Homepage, Roles, Admins];
}

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
