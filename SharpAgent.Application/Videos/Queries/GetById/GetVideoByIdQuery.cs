using MediatR;
using SharpAgent.Application.Videos.Common;

namespace SharpAgent.Application.Videos.Queries.GetById;

public class GetVideoByIdQuery : IRequest<VideoDto>
{
    public Guid Id { get; set; }
}
