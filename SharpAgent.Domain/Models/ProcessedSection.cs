namespace SharpAgent.Domain.Models;

public class ProcessedSection
{
    public ProcessedSection()
    {
        Paragraphs = new List<string>();
    }

    public string? SectionHeading { get; set; }
    public string? PageHeader { get; set; }
    public string? Title { get; set; }
    public List<string> Paragraphs { get; set; }
}
