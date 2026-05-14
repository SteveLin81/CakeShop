namespace CakeShop.Core.DTOs;

public class CartItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductNameEn { get; set; } = string.Empty;
    public string ProductNameJa { get; set; } = string.Empty;
    public string ProductNameZhCn { get; set; } = string.Empty;
    public string ProductNameTh { get; set; } = string.Empty;
    public string ProductNameKo { get; set; } = string.Empty;
    public string ProductNameVi { get; set; } = string.Empty;
    public string ProductNameMs { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal SubTotal => UnitPrice * Quantity;
}

public class CartDto
{
    public string SessionId { get; set; } = string.Empty;
    public List<CartItemDto> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.SubTotal);
    public int ItemCount => Items.Sum(i => i.Quantity);
}

public class AddToCartRequest
{
    public string SessionId { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 1;
}

public class UpdateCartItemRequest
{
    public int Quantity { get; set; }
}

public class CartOperationResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public CartDto? Cart { get; set; }
}
