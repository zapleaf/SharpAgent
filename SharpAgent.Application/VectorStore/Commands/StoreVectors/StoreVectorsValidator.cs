using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAgent.Application.VectorStore.Commands.StoreVectors;

public class StoreVectorsValidator : AbstractValidator<StoreVectorsCommand>
{
    public StoreVectorsValidator()
    {
        RuleFor(x => x.Vectors)
            .NotEmpty().WithMessage("Vectors are required")
            .Must(vectors => vectors.All(v => v.Values != null && v.Values.Length > 0))
            .WithMessage("All vectors must have values");

        RuleFor(x => x.Namespace)
            .NotEmpty().WithMessage("Namespace is required")
            .MaximumLength(100).WithMessage("Namespace cannot exceed 100 characters");
    }
}
