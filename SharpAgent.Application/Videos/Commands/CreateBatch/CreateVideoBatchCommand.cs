using SharpAgent.Domain.Entities;
using MediatR;

namespace SharpAgent.Application.Videos.Commands.CreateBatch;

public class CreateVideoBatchCommand : IRequest<int>
{
    public List<Video> Videos { get; set; } = new List<Video>();
}
