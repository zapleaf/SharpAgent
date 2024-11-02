using FluentValidation;

namespace SharpAgent.Application.ChatCompletion.Queries.SendChatCompletion;

public class SendChatCompletionValidator : AbstractValidator<SendChatCompletionQuery>
{
    public SendChatCompletionValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required");

        RuleFor(x => x.Role)
            .Must(BeValidRole).When(x => x.Role != null)
            .WithMessage("Role must be either 'user', 'system', or 'assistant'");
    }

    private bool BeValidRole(string? role)
    {
        if (role == null) return true;
        return role is "user" or "system" or "assistant";
    }
}
