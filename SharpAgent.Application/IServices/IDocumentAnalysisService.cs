using SharpAgent.Domain.Models;

namespace SharpAgent.Application.IServices;

public interface IDocumentAnalysisService
{
    /// <summary>
    /// Gets the complete JSON representation of the analyzed document from URL
    /// </summary>
    /// <param name="documentUrl">URL of the document to analyze</param>
    /// <returns>JSON string containing the complete document analysis</returns>
    Task<string> GetDocumentJsonFromUrlAsync(string documentUrl);

    /// <summary>
    /// Gets the complete JSON representation of the analyzed document from stream
    /// </summary>
    /// <param name="fileStream">Stream containing the document data</param>
    /// <returns>JSON string containing the complete document analysis</returns>
    Task<string> GetDocumentJsonFromStreamAsync(Stream fileStream);

    /// <summary>
    /// Gets only the paragraphs of the analyzed document from URL
    /// </summary>
    Task<ProcessedDocument> ExtractParagraphsFromUrlAsync(string documentUrl);

    /// <summary>
    /// Gets only the paragraphs of the analyzed document from stream
    /// </summary>
    Task<ProcessedDocument> ExtractParagraphsFromStreamAsync(Stream fileStream);
}
