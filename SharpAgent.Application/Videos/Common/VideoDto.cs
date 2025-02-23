namespace SharpAgent.Application.Videos.Common;

public class VideoDto
{
    public int Id { get; set; }
    public string YTId { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public DateTime? PublishedAt { get; set; }
    public int? Duration { get; set; }
    public bool WasWatched { get; set; }
    public int ViewCount { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public decimal RatioAvgViews { get; set; }
    public string? Notes { get; set; }

    // Added channel properties
    public int? ChannelId { get; set; }
    public string? ChannelTitle { get; set; }
    public ulong? ChannelSubscriberCount { get; set; }
}
