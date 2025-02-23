using MediatR;

namespace SharpAgent.Application.Channels.Commands.UpdateNotes;

public class UpdateChannelNotesCommand : IRequest<bool>
{
    public Guid ChannelId { get; set; }
    public string Notes { get; set; } = string.Empty;
}