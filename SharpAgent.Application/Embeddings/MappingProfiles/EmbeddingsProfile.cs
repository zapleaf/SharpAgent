using AutoMapper;
using SharpAgent.Application.Embeddings.Queries.CreateEmbeddings;
using SharpAgent.Application.Models;

namespace SharpAgent.Application.Embeddings.MappingProfiles;

public class EmbeddingsProfile : Profile
{
    public EmbeddingsProfile()
    {
        CreateMap<CreateEmbeddingsResponse, CreateEmbeddingsResult>();
    }
}
