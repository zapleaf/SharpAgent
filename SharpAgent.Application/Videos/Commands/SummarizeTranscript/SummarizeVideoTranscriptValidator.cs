using FluentValidation;

namespace SharpAgent.Application.Videos.Commands.SummarizeTranscript;

public class SummarizeVideoTranscriptValidator : AbstractValidator<SummarizeVideoTranscriptCommand>
{
    public SummarizeVideoTranscriptValidator()
    {
        RuleFor(x => x.AiSummaryId)
            .NotEmpty().WithMessage("AiSummary ID is required");
    }
}