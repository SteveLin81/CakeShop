using CakeShop.Core.DTOs;

namespace CakeShop.Core.Interfaces;

public interface IContactService
{
    Task<ContactFormResponse> SubmitFormAsync(ContactFormDto form);
}
