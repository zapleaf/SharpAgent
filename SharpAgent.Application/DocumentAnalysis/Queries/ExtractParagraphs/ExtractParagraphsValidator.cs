using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAgent.Application.DocumentAnalysis.Queries.ExtractParagraphs;

public class ExtractParagraphsValidator : AbstractValidator<ExtractParagraphsQuery>
{
    public ExtractParagraphsValidator()
    {
        RuleFor(x => x.DocumentUrl)
            .NotEmpty().WithMessage("Document URL is required")
            .Must(BeAValidUrl).WithMessage("A valid URL must be provided");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}
