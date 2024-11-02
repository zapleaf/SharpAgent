using AutoMapper;
using SharpAgent.Application.ChatCompletion.Queries.SendChatCompletion;
using SharpAgent.Domain.Models;

namespace SharpAgent.Application.ChatCompletion.MappingProfiles;

public class ChatCompletionProfile : Profile
{
    public ChatCompletionProfile()
    {
        CreateMap<ChatResponse, SendChatCompletionResponse>();
    }
}
