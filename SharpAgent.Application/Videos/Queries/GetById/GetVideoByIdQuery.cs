using MediatR;
using SharpAgent.Application.Videos.Common;

namespace SharpAgent.Application.Videos.Queries.GetById;

public class GetVideoByIdQuery : IRequest<VideoResponse>
{
    public Guid Id { get; set; }
}
