using AutoMapper;
using MediatR;

using SharpAgent.Application.AiAnalysises.Common;
using SharpAgent.Application.IRepositories;
using SharpAgent.Domain.Entities;

namespace SharpAgent.Application.AiAnalysises.Commands.Create;

public class CreateAiAnalysisHandler : IRequestHandler<CreateAiAnalysisCommand, AiAnalysisDto>
{
    private readonly IAiAnalysisRepository _aiAnalysisRepository;
    private readonly IMapper _mapper;

    public CreateAiAnalysisHandler(IAiAnalysisRepository aiAnalysisRepository, IMapper mapper)
    {
        _aiAnalysisRepository = aiAnalysisRepository;
        _mapper = mapper;
    }

    public async Task<AiAnalysisDto> Handle(CreateAiAnalysisCommand request, CancellationToken cancellationToken)
    {
        var analysis = _mapper.Map<AiAnalysis>(request);
        var result = await _aiAnalysisRepository.Create(analysis);
        return _mapper.Map<AiAnalysisDto>(result);
    }
}
