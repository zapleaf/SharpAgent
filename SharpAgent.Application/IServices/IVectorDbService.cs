using SharpAgent.Domain.Models;

namespace SharpAgent.Application.IServices;

public interface IVectorDbService
{
    Task<uint> UpsertVectors(List<EmbeddingVector> embeddings, string vectorNamespace);
}
