using SharpAgent.Application.Categories.Queries.GetAll;
using SharpAgent.Application.Videos.Common;

namespace SharpAgent.Application.Channels.Common;

public class ChannelResponse
{
    public Guid Id { get; set; }
    public string YTId { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ThumbnailURL { get; set; }
    public DateTime PublishedAt { get; set; }
    public DateTime? LastCheckDate { get; set; }
    public ulong? SubscriberCount { get; set; }
    public ulong? VideoCount { get; set; }
    public string? Notes { get; set; }

    public int AvgViews { get; set; }
    public int TrackedVideos { get; set; } = 0;
    public List<CategoryResponse> Categories { get; set; } = new();
}
