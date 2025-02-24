using SharpAgent.Application.Videos.Common;
using MediatR;

namespace SharpAgent.Application.Videos.Queries.GetByCategory;

public class GetVideosByCategoryQuery : IRequest<List<VideoResponse>>
{
    public Guid CategoryId { get; set; }
}
