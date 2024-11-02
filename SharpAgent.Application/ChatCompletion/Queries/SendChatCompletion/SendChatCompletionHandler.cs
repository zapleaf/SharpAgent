using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SharpAgent.Application.IServices;

namespace SharpAgent.Application.ChatCompletion.Queries.SendChatCompletion;

public class SendChatCompletionHandler
    : IRequestHandler<SendChatCompletionQuery, SendChatCompletionResponse>
{
    private readonly IOpenAIChatService _chatService;
    private readonly IMapper _mapper;
    private readonly ILogger<SendChatCompletionHandler> _logger;

    public SendChatCompletionHandler(
        IOpenAIChatService chatService,
        IMapper mapper,
        ILogger<SendChatCompletionHandler> logger)
    {
        _chatService = chatService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SendChatCompletionResponse> Handle(
        SendChatCompletionQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Sending chat completion request");

            var response = await _chatService.SendChatCompletionAsync(
                request.Content,
                request.Role);

            var result = _mapper.Map<SendChatCompletionResponse>(response);

            _logger.LogInformation("Successfully received chat completion response");

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing chat completion request");
            throw;
        }
    }
}
