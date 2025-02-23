using AutoMapper;
using MediatR;

using SharpAgent.Application.IRepositories;
using SharpAgent.Domain.Entities;

namespace SharpAgent.Application.AiSummaries.Create;

public class CreateAiSummaryHandler : IRequestHandler<CreateAiSummaryCommand, Guid>
{
    private readonly IAiSummaryRepository _aiSummaryRepository;
    private readonly IMapper _mapper;

    public CreateAiSummaryHandler(IAiSummaryRepository aiSummaryRepository, IMapper mapper)
    {
        _aiSummaryRepository = aiSummaryRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateAiSummaryCommand request, CancellationToken cancellationToken)
    {
        var aiSummary = _mapper.Map<AiSummary>(request);
        var result = await _aiSummaryRepository.Create(aiSummary);
        return result.Id;
    }
}
