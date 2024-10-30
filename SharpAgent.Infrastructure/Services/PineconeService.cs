using Microsoft.Extensions.Configuration;

using SharpAgent.Application.IServices;

using SharpAgent.Domain.Models;

using Pinecone;

namespace SharpAgent.Infrastructure.Services
{
    public class PineconeService : IVectorDbService
    {
        private readonly IConfiguration _config;
        private readonly PineconeClient _pineconeClient;
        private readonly IndexClient _indexClient;

        public PineconeService(IConfiguration config)
        {
            _config = config;

            // Used to interact with the Pinecone service based on the provided api key
            _pineconeClient = new PineconeClient(_config["Pinecone:ApiKey"]);

            // Used to interact with a specific index in the Pinecone vector database.
            // If you are on the free account you will only be able to create 1 index
            // This needs to match the index name you setup in Pinecone
            _indexClient = _pineconeClient.Index(_config["Pinecone:Index"]);
        }

        public async Task UpsertVectors(List<EmbeddingVector> embeddings, string vectorNamespace)
        {
            var pineconeVectors = ConvertVectors(embeddings);
            await UpsertVectors(pineconeVectors, vectorNamespace);
        }

        private static List<Vector> ConvertVectors(List<EmbeddingVector> embeddings)
        {
            var pineconeVectors = new List<Vector>();

            foreach (var embedding in embeddings)
            {
                // Create new dictionary with all existing metadata
                var enhancedMetadata = new Dictionary<string, string>(embedding.Metadata)
                {
                    // Add the filename to metadata
                    ["fileName"] = embedding.FileName
                };

                var metadataPairs = enhancedMetadata.Select(kvp =>
                    new KeyValuePair<string, MetadataValue?>(
                        kvp.Key,
                        new MetadataValue(kvp.Value)
                    )
                );

                var vector = new Vector
                {
                    Id = embedding.Id.ToString(),
                    Values = embedding.Values,
                    Metadata = new Metadata(metadataPairs)
                };

                pineconeVectors.Add(vector);
            }

            return pineconeVectors;
        }

        /// <summary>
        /// Performs both insert AND update.
        /// If a vector doesn't exist (by id), it creates a new one
        /// If it already exists, it overwrites it completely
        /// Requires the full vector data (embeddings + metadata) to be provided
        /// </summary>
        private async Task<uint> UpsertVectors(List<Vector> vectors, string vectorNamespace)
        {
            try
            {
                // Removed Sparse values
                var response = await _indexClient.UpsertAsync(
                new UpsertRequest
                {
                    Vectors = vectors,
                    Namespace = vectorNamespace
                }
                );

                return response.UpsertedCount ?? 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0;
        }
    }
}
