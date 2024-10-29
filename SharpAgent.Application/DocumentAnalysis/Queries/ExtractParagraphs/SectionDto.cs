using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAgent.Application.DocumentAnalysis.Queries.ExtractParagraphs;

public class SectionDto
{
    public string? SectionHeading { get; set; }
    public string? PageHeader { get; set; }
    public string? Title { get; set; }
    public List<string> Paragraphs { get; set; } = new();
}
