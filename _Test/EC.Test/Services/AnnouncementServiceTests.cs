using CakeShop.Core.Interfaces;
using EC.CommonService.Services;
using EC.Entities.Models;
using FluentAssertions;
using Moq;

namespace EC.Test.Services;

public class AnnouncementServiceTests
{
    private readonly Mock<IAnnouncementRepository> _repoMock = new();
    private readonly AnnouncementService           _sut;

    public AnnouncementServiceTests()
        => _sut = new AnnouncementService(_repoMock.Object);

    [Fact]
    public async Task GetActiveAnnouncementAsync_NoAnnouncement_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetActiveAsync()).ReturnsAsync((Announcement?)null);

        var result = await _sut.GetActiveAnnouncementAsync();

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetActiveAnnouncementAsync_WithAnnouncement_ReturnsMappedDto()
    {
        var ann = BuildAnnouncement();
        _repoMock.Setup(r => r.GetActiveAsync()).ReturnsAsync(ann);

        var dto = await _sut.GetActiveAnnouncementAsync();

        dto.Should().NotBeNull();
        dto!.Content.Should().Be("公告內容");
        dto.ContentEn.Should().Be("Announcement");
        dto.ContentJa.Should().Be("お知らせ");
        dto.ContentZhCn.Should().Be("公告内容");
        dto.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetActiveAnnouncementAsync_NullContentTh_FallsBackToContentEn()
    {
        var ann = BuildAnnouncement();
        ann.ContentTh = null;
        _repoMock.Setup(r => r.GetActiveAsync()).ReturnsAsync(ann);

        var dto = await _sut.GetActiveAnnouncementAsync();

        dto!.ContentTh.Should().Be(ann.ContentEn);
    }

    [Fact]
    public async Task GetActiveAnnouncementAsync_NullContentKo_FallsBackToContentEn()
    {
        var ann = BuildAnnouncement();
        ann.ContentKo = null;
        _repoMock.Setup(r => r.GetActiveAsync()).ReturnsAsync(ann);

        var dto = await _sut.GetActiveAnnouncementAsync();

        dto!.ContentKo.Should().Be(ann.ContentEn);
    }

    [Fact]
    public async Task GetActiveAnnouncementAsync_NullContentVi_FallsBackToContentEn()
    {
        var ann = BuildAnnouncement();
        ann.ContentVi = null;
        _repoMock.Setup(r => r.GetActiveAsync()).ReturnsAsync(ann);

        var dto = await _sut.GetActiveAnnouncementAsync();

        dto!.ContentVi.Should().Be(ann.ContentEn);
    }

    [Fact]
    public async Task GetActiveAnnouncementAsync_NullContentMs_FallsBackToContentEn()
    {
        var ann = BuildAnnouncement();
        ann.ContentMs = null;
        _repoMock.Setup(r => r.GetActiveAsync()).ReturnsAsync(ann);

        var dto = await _sut.GetActiveAnnouncementAsync();

        dto!.ContentMs.Should().Be(ann.ContentEn);
    }

    [Fact]
    public async Task GetActiveAnnouncementAsync_AllLangFieldsSet_ReturnAllFields()
    {
        var ann = BuildAnnouncement();
        _repoMock.Setup(r => r.GetActiveAsync()).ReturnsAsync(ann);

        var dto = await _sut.GetActiveAnnouncementAsync();

        dto!.ContentTh.Should().Be("ประกาศ");
        dto.ContentKo.Should().Be("공지사항");
        dto.ContentVi.Should().Be("Thông báo");
        dto.ContentMs.Should().Be("Pengumuman");
    }

    // ── Helper ───────────────────────────────────────────────────────

    private static Announcement BuildAnnouncement() => new()
    {
        Id         = 1,
        Content    = "公告內容",
        ContentEn  = "Announcement",
        ContentJa  = "お知らせ",
        ContentZhCn = "公告内容",
        ContentTh  = "ประกาศ",
        ContentKo  = "공지사항",
        ContentVi  = "Thông báo",
        ContentMs  = "Pengumuman",
        IsActive   = true
    };
}
