using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CakeShop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    /// <summary>取得購物車內容</summary>
    [HttpGet("{sessionId}")]
    public async Task<ActionResult<CartDto>> GetCart(string sessionId)
        => Ok(await _cartService.GetCartAsync(sessionId));

    /// <summary>新增商品至購物車</summary>
    [HttpPost]
    public async Task<ActionResult<CartOperationResponse>> AddToCart([FromBody] AddToCartRequest request)
    {
        var result = await _cartService.AddToCartAsync(request);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>更新購物車商品數量</summary>
    [HttpPut("{sessionId}/items/{itemId:int}")]
    public async Task<ActionResult<CartOperationResponse>> UpdateQuantity(
        string sessionId, int itemId, [FromBody] UpdateCartItemRequest request)
    {
        var result = await _cartService.UpdateQuantityAsync(sessionId, itemId, request.Quantity);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>移除購物車商品</summary>
    [HttpDelete("{sessionId}/items/{itemId:int}")]
    public async Task<ActionResult<CartOperationResponse>> RemoveItem(string sessionId, int itemId)
    {
        var result = await _cartService.RemoveFromCartAsync(sessionId, itemId);
        return Ok(result);
    }

    /// <summary>清空購物車</summary>
    [HttpDelete("{sessionId}")]
    public async Task<ActionResult<CartOperationResponse>> ClearCart(string sessionId)
        => Ok(await _cartService.ClearCartAsync(sessionId));
}
