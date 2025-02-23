using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using SharpAgent.Domain.Common;

namespace SharpAgent.Domain.Entities;

public class Video : BaseEntity
{
    [Required]
    public string YTId { get; set; } = string.Empty;
    public string? YTChannelId { get; set; }
    public string? Title { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Description { get; set; }
    public DateTime? PublishedAt { get; set; }
    public int? Duration { get; set; }

    public bool WasWatched { get; set; } = false;
    public string? Notes { get; set; }

    public Guid ChannelId { get; set; }
    public virtual Channel? Channel { get; set; }

    public int ViewCount { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }

    [Column(TypeName = "decimal(10,8)")]
    public decimal RatioAvgViews { get; set; }
}
