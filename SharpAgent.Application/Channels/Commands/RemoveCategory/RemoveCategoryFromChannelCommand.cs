using MediatR;

namespace SharpAgent.Application.Channels.Commands.RemoveCategory;

public class RemoveCategoryFromChannelCommand : IRequest<bool>
{
    public Guid CategoryId { get; set; }
    public Guid ChannelId { get; set; }
}