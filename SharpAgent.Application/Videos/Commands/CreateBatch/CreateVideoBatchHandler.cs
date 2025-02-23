using SharpAgent.Application.IRepositories;
using MediatR;

namespace SharpAgent.Application.Videos.Commands.CreateBatch;

public class CreateVideoBatchHandler : IRequestHandler<CreateVideoBatchCommand, int>
{
    private readonly IVideoRepository _videoRepository;

    public CreateVideoBatchHandler(IVideoRepository videoRepository)
    {
        _videoRepository = videoRepository;
    }

    public async Task<int> Handle(CreateVideoBatchCommand request, CancellationToken cancellationToken)
    {
        return await _videoRepository.Create(request.Videos);
    }
}
