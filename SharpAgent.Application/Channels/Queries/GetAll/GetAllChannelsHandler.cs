using AutoMapper;
using SharpAgent.Application.IRepositories;
using SharpAgent.Application.Channels.Common;
using MediatR;
using Azure;

namespace SharpAgent.Application.Channels.Queries.GetAll;

public class GetAllChannelsHandler : IRequestHandler<GetAllChannelsQuery, List<ChannelResponse>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IMapper _mapper;

    public GetAllChannelsHandler(IChannelRepository channelRepository, IMapper mapper)
    {
        _channelRepository = channelRepository;
        _mapper = mapper;
    }

    public async Task<List<ChannelResponse>> Handle(GetAllChannelsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var channels = request.IncludeVideos && request.IncludeCategories
                ? await _channelRepository.GetWithCategoriesAndVideos()
                : request.IncludeCategories
                    ? await _channelRepository.GetWithCategories()
                    : await _channelRepository.GetAll();

            var response = _mapper.Map<List<ChannelResponse>>(channels);

            return response;
        }
        catch (Exception ex)
        {
            string err = ex.Message;
        }

        return null;
    }
}
