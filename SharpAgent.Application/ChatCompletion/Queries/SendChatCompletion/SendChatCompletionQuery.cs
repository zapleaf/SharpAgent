using MediatR;
using SharpAgent.Domain.Models;

namespace SharpAgent.Application.ChatCompletion.Queries.SendChatCompletion;

public class SendChatCompletionQuery : IRequest<SendChatCompletionResponse>
{
    public required string Content { get; init; }
    public string? Role { get; init; }
}