namespace SharpAgent.Domain.Models;

public class ProcessedDocument
{
    public ProcessedDocument()
    {
        Sections = new List<ProcessedSection>();
    }

    public List<ProcessedSection> Sections { get; set; }
}
