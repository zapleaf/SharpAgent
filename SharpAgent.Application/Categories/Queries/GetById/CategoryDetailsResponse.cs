using SharpAgent.Application.Categories.Common;

namespace SharpAgent.Application.Categories.Queries.GetById;

public class CategoryDetailsResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
    public List<CategoryChannelResponse> Channels { get; set; } = new();
}