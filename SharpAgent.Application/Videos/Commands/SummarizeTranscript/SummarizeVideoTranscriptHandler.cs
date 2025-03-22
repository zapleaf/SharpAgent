using MediatR;
using Microsoft.Extensions.Logging;
using SharpAgent.Application.IRepositories;
using SharpAgent.Application.IServices;
using SharpAgent.Domain.Entities;

namespace SharpAgent.Application.Videos.Commands.SummarizeTranscript;

public class SummarizeVideoTranscriptHandler : IRequestHandler<SummarizeVideoTranscriptCommand, Guid?>
{
    private readonly IAiSummaryRepository _aiSummaryRepository;
    private readonly IOpenAIChatService _chatService;
    private readonly ILogger<SummarizeVideoTranscriptHandler> _logger;

    public SummarizeVideoTranscriptHandler(
        IAiSummaryRepository aiSummaryRepository,
        IOpenAIChatService chatService,
        ILogger<SummarizeVideoTranscriptHandler> logger)
    {
        _aiSummaryRepository = aiSummaryRepository ?? throw new ArgumentNullException(nameof(aiSummaryRepository));
        _chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Guid?> Handle(SummarizeVideoTranscriptCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // 1. Get the AiSummary from the database
            var aiSummary = await _aiSummaryRepository.Get(request.AiSummaryId);
            if (aiSummary == null)
            {
                _logger.LogWarning($"AiSummary with ID {request.AiSummaryId} not found");
                return null;
            }

            // 2. Check if a transcript exists
            if (string.IsNullOrWhiteSpace(aiSummary.Transcript))
            {
                _logger.LogWarning($"No transcript found for AiSummary with ID {request.AiSummaryId}");
                return null;
            }

            // 3. Prepare the prompt context for summarization
            string promptContext = $"This is a transcript from a YouTube video. Please summarize the main topics and key points discussed in this video:\n\n{aiSummary.Transcript}";

            // 4. Generate the summary using the simplified method
            string summary = await _chatService.GenerateSummaryAsync(promptContext, 800);

            if (string.IsNullOrWhiteSpace(summary))
            {
                _logger.LogWarning($"No summary generated for AiSummary with ID {request.AiSummaryId}");
                return null;
            }

            // 5. Update the AiSummary with the summary
            aiSummary.Summary = summary;
            aiSummary.Provider = "OpenAI";
            aiSummary.Model = _chatService.GetType().Name; // This is a simplification, ideally we'd get the actual model name

            // 6. Save the updated AiSummary
            var updatedSummary = await _aiSummaryRepository.Update(aiSummary);
            _logger.LogInformation($"Updated AiSummary with ID {updatedSummary.Id} with summary from OpenAI");

            return updatedSummary.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error summarizing transcript for AiSummary ID {request.AiSummaryId}: {ex.Message}");
            throw;
        }
    }
}