using SharpAgent.Application.Videos.Common;
using MediatR;

namespace SharpAgent.Application.Videos.Queries.GetAll;

public class GetAllVideosQuery : IRequest<List<VideoResponse>>
{
}
