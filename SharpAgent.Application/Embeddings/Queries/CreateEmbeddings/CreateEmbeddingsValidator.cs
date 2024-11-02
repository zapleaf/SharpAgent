using FluentValidation;

namespace SharpAgent.Application.Embeddings.Queries.CreateEmbeddings;

public class CreateEmbeddingsValidator : AbstractValidator<CreateEmbeddingsQuery>
{
    public CreateEmbeddingsValidator()
    {
        RuleFor(x => x.TextSections)
            .NotEmpty().WithMessage("Text sections are required")
            .Must(sections => sections.All(s => !string.IsNullOrWhiteSpace(s)))
            .WithMessage("All text sections must contain content");
    }
}
