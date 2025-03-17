namespace SharpAgent.Application.Categories.Common;

public class CategoryChannelResponse
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? ThumbnailURL { get; set; }
    public ulong? SubscriberCount { get; set; }
}
