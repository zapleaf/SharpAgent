using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAgent.Application.IServices;

public interface IEmbeddingService
{
    Task<List<ReadOnlyMemory<float>>> CreateEmbeddingsAsync(List<string> inputs);
}
