using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SharpAgent.Application.AiSummaries.Common;
using SharpAgent.Application.IRepositories;
using SharpAgent.Application.Videos.Commands.RetrieveTranscript;
using SharpAgent.Application.Videos.Commands.SummarizeTranscript;

namespace SharpAgent.Application.Videos.Commands.GetSummary;

public class GetVideoSummaryHandler : IRequestHandler<GetVideoSummaryCommand, AiSummaryResponse?>
{
    private readonly IAiSummaryRepository _aiSummaryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetVideoSummaryHandler> _logger;

    public GetVideoSummaryHandler(
        IAiSummaryRepository aiSummaryRepository,
        IMapper mapper,
        ILogger<GetVideoSummaryHandler> logger)
    {
        _aiSummaryRepository = aiSummaryRepository ?? throw new ArgumentNullException(nameof(aiSummaryRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<AiSummaryResponse?> Handle(GetVideoSummaryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // 1. Check if we already have a summary for this video
            var existingSummary = await _aiSummaryRepository.GetMostRecentByVideoId(request.VideoId);

            // 2. If we have a summary with content, return it
            if (existingSummary != null && !string.IsNullOrWhiteSpace(existingSummary.Summary))
            {
                _logger.LogInformation($"Found existing summary for video {request.VideoId}");
                return _mapper.Map<AiSummaryResponse>(existingSummary);
            }

            // No valid summary found
            _logger.LogInformation($"No valid summary found for video {request.VideoId}");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting summary for video {request.VideoId}: {ex.Message}");
            throw;
        }
    }
}