using Azure;
using Azure.AI.DocumentIntelligence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SharpAgent.Application.IServices;
using SharpAgent.Domain.Models;
using System.Text.Json;

namespace SharpAgent.Infrastructure.Services;

internal class AzureDocumentService : IDocumentAnalysisService
{
    private readonly DocumentIntelligenceClient _client;
    private readonly ILogger<AzureDocumentService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public AzureDocumentService(
        IConfiguration configuration,
        ILogger<AzureDocumentService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        var endpoint = configuration["Azure:DocumentIntelligence:Endpoint"]
            ?? throw new InvalidOperationException("Azure:DocumentIntelligence:Endpoint configuration is missing");
        var key = configuration["Azure:DocumentIntelligence:Key"]
            ?? throw new InvalidOperationException("Azure:DocumentIntelligence:Key configuration is missing");

        var credential = new AzureKeyCredential(key);
        _client = new DocumentIntelligenceClient(new Uri(endpoint), credential);

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<string> GetDocumentJsonFromUrlAsync(string documentUrl)
    {
        var result = await AnalyzeDocumentFromUrlAsync(documentUrl);
        return SerializeAnalyzeResult(result);
    }

    public async Task<string> GetDocumentJsonFromStreamAsync(Stream fileStream)
    {
        var result = await AnalyzeDocumentFromStreamAsync(fileStream);
        return SerializeAnalyzeResult(result);
    }

    private async Task<AnalyzeResult> AnalyzeDocumentFromUrlAsync(string documentUrl)
    {
        try
        {
            _logger.LogInformation($"Analyzing document from URL: {documentUrl}");

            var content = new AnalyzeDocumentContent
            {
                UrlSource = new Uri(documentUrl)
            };

            Operation<AnalyzeResult> operation = await _client.AnalyzeDocumentAsync(
                WaitUntil.Completed,
                "prebuilt-layout",
                content);

            var result = operation.Value;
            _logger.LogInformation($"Successfully analyzed document with {result.Pages.Count} pages");

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error analyzing document from URL: {ex.Message}");
            throw;
        }
    }

    private async Task<AnalyzeResult> AnalyzeDocumentFromStreamAsync(Stream fileStream)
    {
        try
        {
            _logger.LogInformation("Analyzing document from stream");

            var content = new AnalyzeDocumentContent
            {
                Base64Source = await BinaryData.FromStreamAsync(fileStream)
            };

            Operation<AnalyzeResult> operation = await _client.AnalyzeDocumentAsync(
                WaitUntil.Completed,
                "prebuilt-layout",
                content);

            var result = operation.Value;
            _logger.LogInformation($"Successfully analyzed document with {result.Pages.Count} pages");

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error analyzing document from stream: {ex.Message}");
            throw;
        }
    }

    private string SerializeAnalyzeResult(AnalyzeResult result)
    {
        try
        {
            var documentData = new
            {
                Pages = result.Pages.Select(page => new
                {
                    PageNumber = page.PageNumber,
                    Lines = page.Lines.Select(line => new
                    {
                        Content = line.Content,
                        BoundingBox = line.Polygon.ToArray(),
                        // Removed Words as it's not available in the current SDK
                    }),
                    SelectionMarks = page.SelectionMarks.Select(mark => new
                    {
                        State = mark.State,
                        BoundingBox = mark.Polygon.ToArray()
                    })
                }),

                Figures = result.Figures.Select(figure => new
                {
                    PageNumber = figure.BoundingRegions.FirstOrDefault()?.PageNumber,
                    BoundingBox = figure.BoundingRegions.FirstOrDefault()?.Polygon.ToArray()
                }),

                KeyValuePairs = result.KeyValuePairs.Select(kvp => new
                {
                    Key = new
                    {
                        Content = kvp.Key.Content,
                        BoundingBox = kvp.Key.BoundingRegions.FirstOrDefault()?.Polygon.ToArray()
                    },
                    Value = new
                    {
                        Content = kvp.Value.Content,
                        BoundingBox = kvp.Value.BoundingRegions.FirstOrDefault()?.Polygon.ToArray()
                    },
                    Confidence = kvp.Confidence
                }),

                Tables = result.Tables.Select(table => new
                {
                    RowCount = table.RowCount,
                    ColumnCount = table.ColumnCount,
                    Cells = table.Cells.Select(cell => new
                    {
                        RowIndex = cell.RowIndex,
                        ColumnIndex = cell.ColumnIndex,
                        Content = cell.Content,
                        BoundingBox = cell.BoundingRegions.ToArray(),
                        RowSpan = cell.RowSpan,
                        ColumnSpan = cell.ColumnSpan
                    })
                }),

                Paragraphs = result.Paragraphs.Select(para => new
                {
                    Content = para.Content,
                    BoundingRegions = para.BoundingRegions.Select(region => new
                    {
                        PageNumber = region.PageNumber,
                        BoundingBox = region.Polygon.ToArray()
                    }),
                    Role = para.Role
                }),

                Styles = result.Styles.Select(style => new
                {
                    IsHandwritten = style.IsHandwritten,
                    Confidence = style.Confidence,
                    Spans = style.Spans.Select(span => new
                    {
                        Offset = span.Offset,
                        Length = span.Length
                    })
                }),

                Languages = result.Languages.Select(lang => new
                {
                    Language = lang.Locale,
                    Confidence = lang.Confidence
                })
            };

            return JsonSerializer.Serialize(documentData, _jsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error serializing analyze result to JSON");
            throw;
        }
    }

    public async Task<ProcessedDocument> ExtractParagraphsFromUrlAsync(string documentUrl)
    {
        var result = await AnalyzeDocumentFromUrlAsync(documentUrl);
        return ProcessParagraphs(result.Paragraphs);
    }

    public async Task<ProcessedDocument> ExtractParagraphsFromStreamAsync(Stream fileStream)
    {
        var result = await AnalyzeDocumentFromStreamAsync(fileStream);
        return ProcessParagraphs(result.Paragraphs);
    }

    private ProcessedDocument ProcessParagraphs(IReadOnlyList<DocumentParagraph> paragraphs)
    {
        var processedDocument = new ProcessedDocument();
        ProcessedSection? currentSection = null;
        string? currentPageHeader = null;
        string? currentTitle = null;

        foreach (var paragraph in paragraphs)
        {
            switch (paragraph.Role?.ToString().ToLower())
            {
                case "pageheader":
                    currentPageHeader = paragraph.Content;
                    // Update current section if it exists
                    if (currentSection != null)
                    {
                        currentSection.PageHeader = currentPageHeader;
                    }
                    break;

                case "title":
                    currentTitle = paragraph.Content;
                    // Update current section if it exists
                    if (currentSection != null)
                    {
                        currentSection.Title = currentTitle;
                    }
                    break;

                case "sectionheading":
                    // Create new section with current metadata
                    currentSection = new ProcessedSection
                    {
                        SectionHeading = paragraph.Content,
                        PageHeader = currentPageHeader,
                        Title = currentTitle
                    };
                    processedDocument.Sections.Add(currentSection);
                    break;

                default:
                    // If no section exists yet, create default section
                    if (currentSection == null)
                    {
                        currentSection = new ProcessedSection
                        {
                            PageHeader = currentPageHeader,
                            Title = currentTitle
                        };
                        processedDocument.Sections.Add(currentSection);
                    }

                    // Only add non-empty paragraphs
                    if (!string.IsNullOrWhiteSpace(paragraph.Content))
                    {
                        currentSection.Paragraphs.Add(paragraph.Content);
                    }
                    break;
            }
        }

        // Handle empty document
        if (!processedDocument.Sections.Any())
        {
            processedDocument.Sections.Add(new ProcessedSection
            {
                PageHeader = currentPageHeader,
                Title = currentTitle
            });
        }

        return processedDocument;
    }
}