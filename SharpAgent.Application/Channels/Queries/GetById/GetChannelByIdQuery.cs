using MediatR;
using SharpAgent.Application.Channels.Common;

namespace SharpAgent.Application.Channels.Queries.GetById;

public record GetChannelByIdQuery(Guid Id) : IRequest<ChannelResponse>;
