using MediatR;
using SharpAgent.Application.Channels.Common;

namespace SharpAgent.Application.Channels.Queries.GetByCategory;

public class GetChannelsByCategoryQuery : IRequest<List<ChannelDto>>
{
    public Guid CategoryId { get; set; }
}
