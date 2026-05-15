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
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email    { get; set; } = string.Empty;
}

public class B2cUserUpdateRequest
{
    public string  Email       { get; set; } = string.Empty;
    public string? NewPassword { get; set; }
}

// ── 商品管理 ───────────────────────────────────────────────────────────
public class ProductSaveRequest
{
    public string  Name            { get; set; } = string.Empty;
    public string  NameEn          { get; set; } = string.Empty;
    public string  NameJa          { get; set; } = string.Empty;
    public string  NameZhCn        { get; set; } = string.Empty;
    public string  NameTh          { get; set; } = string.Empty;
    public string  NameKo          { get; set; } = string.Empty;
    public string  NameVi          { get; set; } = string.Empty;
    public string  NameMs          { get; set; } = string.Empty;
    public string  Description     { get; set; } = string.Empty;
    public string  DescriptionEn   { get; set; } = string.Empty;
    public string  DescriptionJa   { get; set; } = string.Empty;
    public string  DescriptionZhCn { get; set; } = string.Empty;
    public string  DescriptionTh   { get; set; } = string.Empty;
    public string  DescriptionKo   { get; set; } = string.Empty;
    public string  DescriptionVi   { get; set; } = string.Empty;
    public string  DescriptionMs   { get; set; } = string.Empty;
    public decimal Price           { get; set; }
    public string  ImageUrl        { get; set; } = string.Empty;
    public int     CategoryId      { get; set; }
    public bool    IsAvailable     { get; set; } = true;
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
