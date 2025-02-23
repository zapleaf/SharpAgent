using MediatR;

namespace SharpAgent.Application.Videos.Commands.UpdateVideo;

public class UpdateVideoNotesCommand : IRequest<bool>
{
    public Guid VideoId { get; set; }
    public string Notes { get; set; } = string.Empty;
}