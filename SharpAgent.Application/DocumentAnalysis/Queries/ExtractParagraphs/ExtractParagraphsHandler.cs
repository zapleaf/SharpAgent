using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SharpAgent.Application.IServices;

namespace SharpAgent.Application.DocumentAnalysis.Queries.ExtractParagraphs;

public class ExtractParagraphsHandler : IRequestHandler<ExtractParagraphsQuery, ExtractParagraphsResponse>
{
    private readonly IDocumentAnalysisService _documentAnalysisService;
    private readonly IMapper _mapper;
    private readonly ILogger<ExtractParagraphsHandler> _logger;

    public ExtractParagraphsHandler(
        IDocumentAnalysisService documentAnalysisService,
        IMapper mapper,
        ILogger<ExtractParagraphsHandler> logger)
    {
        _documentAnalysisService = documentAnalysisService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ExtractParagraphsResponse> Handle(
        ExtractParagraphsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Extracting paragraphs from URL: {Url}", request.DocumentUrl);

            var processedDocument = await _documentAnalysisService
                .ExtractParagraphsFromUrlAsync(request.DocumentUrl);

            var response = _mapper.Map<ExtractParagraphsResponse>(processedDocument);

            _logger.LogInformation("Successfully extracted {SectionCount} sections from document",
                response.Sections.Count);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error extracting paragraphs from URL: {Url}", request.DocumentUrl);
            throw;
        }
    }
}
