using SharpAgent.Domain.Models;

namespace SharpAgent.Application.IServices;

public interface ITranscriptService
{
    Task<TranscriptResult> ScrapeVideoAsync(string videoUrl);
}
