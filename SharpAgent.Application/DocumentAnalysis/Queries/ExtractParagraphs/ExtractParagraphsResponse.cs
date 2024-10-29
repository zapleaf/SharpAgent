namespace SharpAgent.Application.DocumentAnalysis.Queries.ExtractParagraphs;

public class ExtractParagraphsResponse
{
    public List<SectionDto> Sections { get; set; } = new();
}
