using MediatR;

namespace SharpAgent.Application.Channels.Commands.AddCategory;

public class AddCategoryToChannelCommand : IRequest<bool>
{
    public Guid CategoryId { get; set; }
    public Guid ChannelId { get; set; }
}
