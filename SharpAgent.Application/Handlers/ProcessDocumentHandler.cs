using MediatR;
using Microsoft.Extensions.Logging;
using SharpAgent.Application.Commands;
using SharpAgent.Application.Models;
using SharpAgent.Application.IServices;

namespace SharpAgent.Application.Handlers;

public class ProcessDocumentHandler : IRequestHandler<ProcessDocumentCommand, ProcessDocumentResult>
{
    private readonly IDocumentAnalysisService _documentService;
    private readonly ILogger<ProcessDocumentHandler> _logger;

    public ProcessDocumentHandler(
        IDocumentAnalysisService documentService,
        ILogger<ProcessDocumentHandler> logger)
    {
        _documentService = documentService;
        _logger = logger;
    }

    public async Task<ProcessDocumentResult> Handle(ProcessDocumentCommand command, CancellationToken ct)
    {
        _logger.LogInformation($"Processing document from URL: {command.DocumentUrl}");
        var document = await _documentService.ExtractParagraphsFromUrlAsync(command.DocumentUrl);
        return new ProcessDocumentResult(document);
    }
}
