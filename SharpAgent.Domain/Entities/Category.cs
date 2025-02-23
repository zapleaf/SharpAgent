using System.Threading.Channels;

using SharpAgent.Domain.Common;

namespace SharpAgent.Domain.Entities;
public class Category : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    // Relationships
    public virtual ICollection<Channel> Channels { get; set; } = new List<Channel>();
}
