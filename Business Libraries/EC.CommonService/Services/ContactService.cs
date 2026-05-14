using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;

namespace EC.CommonService.Services;

public class ContactService : IContactService
{
    public async Task<ContactFormResponse> SubmitFormAsync(ContactFormDto form)
    {
        await Task.CompletedTask;
        return new ContactFormResponse
        {
            Success = true,
            Message = "感謝您的來信，我們將於 1-2 個工作天內回覆您！"
        };
    }
}
