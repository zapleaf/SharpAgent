using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAgent.Application.DocumentAnalysis.Queries.ExtractParagraphs;

public record ExtractParagraphsQuery : IRequest<ExtractParagraphsResponse>
{
    public required string DocumentUrl { get; init; }
}
