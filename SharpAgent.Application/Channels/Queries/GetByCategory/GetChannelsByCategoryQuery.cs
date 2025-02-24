using MediatR;
using SharpAgent.Application.Channels.Common;

namespace SharpAgent.Application.Channels.Queries.GetByCategory;

public class GetChannelsByCategoryQuery : IRequest<List<ChannelResponse>>
{
    public Guid CategoryId { get; set; }
}
