using CakeShop.Core.DTOs;
using CakeShop.Core.Interfaces;

namespace CakeShop.Business.Services;

public class AnnouncementService : IAnnouncementService
{
    private static readonly AnnouncementDto _announcement = new()
    {
        Content    = "📢 4月30日公休，出貨日期順延一天，敬請見諒。",
        ContentEn  = "📢 We are closed on April 30. Orders will be shipped one day later. We apologize for the inconvenience.",
        ContentJa  = "📢 4月30日は休業となります。出荷日が1日遅れます。ご不便をおかけして申し訳ございません。",
        ContentZhCn = "📢 4月30日公休，发货日期顺延一天，敬请谅解。",
        IsActive   = true
    };

    public Task<AnnouncementDto?> GetActiveAnnouncementAsync()
        => Task.FromResult(_announcement.IsActive ? _announcement : null);
}
