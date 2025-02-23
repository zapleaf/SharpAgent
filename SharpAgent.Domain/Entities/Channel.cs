using System.ComponentModel.DataAnnotations;

using SharpAgent.Domain.Common;

namespace SharpAgent.Domain.Entities;

public class Channel : BaseEntity
{
    [Required]
    public string YTId { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? UploadsPlaylistId { get; set; }
    public string? ThumbnailURL { get; set; }
    public string? Notes { get; set; }
    public ulong? VideoCount { get; set; }
    public ulong? SubscriberCount { get; set; }
    public DateTime PublishedAt { get; set; }
    public DateTime? LastCheckDate { get; set; }

    public int AvgViews { get; set; }
    public int AvgLikes { get; set; }
    public int AvgComments { get; set; }

    //Relationships
    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
