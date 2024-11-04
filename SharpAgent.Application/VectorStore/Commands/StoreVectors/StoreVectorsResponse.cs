
namespace SharpAgent.Application.VectorStore.Commands.StoreVectors;

public class StoreVectorsResponse
{
    public bool Success { get; init; }
    public uint VectorsStored { get; init; }
    public string? Error { get; init; }

    public static StoreVectorsResponse Successful(uint count) => new()
    {
        Success = true,
        VectorsStored = count
    };

    public static StoreVectorsResponse Failed(string error) => new()
    {
        Success = false,
        Error = error
    };
}
