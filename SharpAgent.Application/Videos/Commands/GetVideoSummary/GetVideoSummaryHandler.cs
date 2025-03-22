using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SharpAgent.Application.AiSummaries.Common;
using SharpAgent.Application.IRepositories;
using SharpAgent.Application.Videos.Commands.RetrieveTranscript;
using SharpAgent.Application.Videos.Commands.SummarizeTranscript;

namespace SharpAgent.Application.Videos.Commands.GetSummary;

public class GetVideoSummaryHandler : IRequestHandler<GetVideoSummaryCommand, AiSummaryResponse>
{
    private readonly IAiSummaryRepository _aiSummaryRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<GetVideoSummaryHandler> _logger;

    public GetVideoSummaryHandler(
        IAiSummaryRepository aiSummaryRepository,
        IMediator mediator,
        IMapper mapper,
        ILogger<GetVideoSummaryHandler> logger)
    {
        _aiSummaryRepository = aiSummaryRepository ?? throw new ArgumentNullException(nameof(aiSummaryRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<AiSummaryResponse> Handle(GetVideoSummaryCommand request, CancellationToken cancellationToken)
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

            // 3. If we have a transcript but no summary, generate the summary
            if (existingSummary != null && !string.IsNullOrWhiteSpace(existingSummary.Transcript))
            {
                _logger.LogInformation($"Found transcript but no summary for video {request.VideoId}. Generating summary...");

                var summarizeCommand = new SummarizeVideoTranscriptCommand
                {
                    AiSummaryId = existingSummary.Id
                };

                await _mediator.Send(summarizeCommand, cancellationToken);

                // Refresh the summary from the database
                existingSummary = await _aiSummaryRepository.Get(existingSummary.Id);
                return _mapper.Map<AiSummaryResponse>(existingSummary);
            }

            // 4. If we don't have a transcript yet, retrieve it first, which will also trigger summarization
            _logger.LogInformation($"No transcript found for video {request.VideoId}. Retrieving transcript...");

            var retrieveCommand = new RetrieveVideoTranscriptCommand
            {
                VideoId = request.VideoId
            };

            var summaryId = await _mediator.Send(retrieveCommand, cancellationToken);

            if (summaryId == null)
            {
                _logger.LogWarning($"Failed to retrieve transcript for video {request.VideoId}");
                return null;
            }

            // 5. Get the newly created summary
            var newSummary = await _aiSummaryRepository.Get(summaryId.Value);
            return _mapper.Map<AiSummaryResponse>(newSummary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting summary for video {request.VideoId}: {ex.Message}");
            throw;
        }
    }
}