using MediatR;
using Microsoft.Extensions.Logging;
using SharpAgent.Application.IRepositories;
using SharpAgent.Application.IServices;
using SharpAgent.Domain.Entities;

namespace SharpAgent.Application.Videos.Commands.RetrieveTranscript;

public class RetrieveVideoTranscriptHandler : IRequestHandler<RetrieveVideoTranscriptCommand, Guid?>
{
    private readonly IVideoRepository _videoRepository;
    private readonly IAiSummaryRepository _aiSummaryRepository;
    private readonly ITranscriptService _transcriptService;
    private readonly ILogger<RetrieveVideoTranscriptHandler> _logger;

    public RetrieveVideoTranscriptHandler(
        IVideoRepository videoRepository,
        IAiSummaryRepository aiSummaryRepository,
        ITranscriptService transcriptService,
        ILogger<RetrieveVideoTranscriptHandler> logger)
    {
        _videoRepository = videoRepository ?? throw new ArgumentNullException(nameof(videoRepository));
        _aiSummaryRepository = aiSummaryRepository ?? throw new ArgumentNullException(nameof(aiSummaryRepository));
        _transcriptService = transcriptService ?? throw new ArgumentNullException(nameof(transcriptService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Guid?> Handle(RetrieveVideoTranscriptCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // 1. Get the video from the database
            var video = await _videoRepository.Get(request.VideoId);
            if (video == null)
            {
                _logger.LogWarning($"Video with ID {request.VideoId} not found");
                return null;
            }

            // 2. Check if an AiSummary already exists for this video with a transcript
            var existingSummary = await _aiSummaryRepository.GetMostRecentByVideoId(video.Id);
            if (existingSummary != null && !string.IsNullOrWhiteSpace(existingSummary.Transcript))
            {
                _logger.LogInformation($"Transcript already exists for video {video.Id}");
                return existingSummary.Id;
            }

            // 3. Construct the YouTube URL for the video
            var youtubeUrl = $"https://www.youtube.com/watch?v={video.YTId}";

            // 4. Get transcript from the TranscriptService
            var transcriptResult = await _transcriptService.ScrapeVideoAsync(youtubeUrl);
            if (transcriptResult == null || string.IsNullOrWhiteSpace(transcriptResult.Subtitles))
            {
                _logger.LogWarning($"No transcript could be retrieved for video {video.Id} (YouTube ID: {video.YTId})");
                return null;
            }

            // 5. Create a new AiSummary with the transcript
            var aiSummary = new AiSummary
            {
                VideoId = video.Id,
                PromptVersionId = null,
                Summary = string.Empty,
                Transcript = transcriptResult.Subtitles,
                Provider = string.Empty,
                Model = string.Empty,
                TokensUsed = null
            };

            // 6. Save the AiSummary to the database
            var result = await _aiSummaryRepository.Create(aiSummary);
            _logger.LogInformation($"Created new AiSummary with ID {result.Id} for video {video.Id}");

            return result.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving transcript for video ID {request.VideoId}: {ex.Message}");
            throw;
        }
    }
}
