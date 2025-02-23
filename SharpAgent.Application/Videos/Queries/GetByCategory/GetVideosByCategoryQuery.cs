using SharpAgent.Application.Videos.Common;
using MediatR;

namespace SharpAgent.Application.Videos.Queries.GetByCategory;

public class GetVideosByCategoryQuery : IRequest<List<VideoDto>>
{
    public Guid CategoryId { get; set; }
}
