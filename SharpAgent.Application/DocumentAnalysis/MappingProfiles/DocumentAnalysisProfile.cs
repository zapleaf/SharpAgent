using AutoMapper;
using SharpAgent.Domain.Models;

using SharpAgent.Application.DocumentAnalysis.Queries.ExtractParagraphs;

namespace SharpAgent.Application.DocumentAnalysis.MappingProfiles;

public class DocumentAnalysisProfile : Profile
{
    public DocumentAnalysisProfile()
    {
        CreateMap<ProcessedDocument, ExtractParagraphsResponse>();
        CreateMap<ProcessedSection, SectionDto>();
    }
}
