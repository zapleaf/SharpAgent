using SharpAgent.Application.IRepositories;
using SharpAgent.Application.IServices;
using MediatR;

namespace SharpAgent.Application.Channels.Commands.UpdateStats;

public class UpdateChannelStatsHandler : IRequestHandler<UpdateChannelStatsCommand, bool>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IVideoRepository _videoRepository;
    private readonly IYouTubeApiService _youTubeApiService;

    public UpdateChannelStatsHandler(
        IChannelRepository channelRepository,
        IVideoRepository videoRepository,
        IYouTubeApiService youTubeApiService)
    {
        _channelRepository = channelRepository;
        _videoRepository = videoRepository;
        _youTubeApiService = youTubeApiService;
    }

    public async Task<bool> Handle(UpdateChannelStatsCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.Get(request.ChannelId);
        if (channel == null)
            return false;

        var channelStats = await _youTubeApiService.GetChannelStats(channel.YTId);
        if (channelStats == null)
            return false;

        channel.SubscriberCount = channelStats.SubscriberCount;
        channel.VideoCount = channelStats.VideoCount;
        channel.AvgViews = channel.Videos.Sum(v => v.ViewCount) / channel.Videos.Count;
        await _channelRepository.Update(channel);

        if (channel.AvgViews > 0)
        {
            foreach (var video in channel.Videos)
            {
                video.RatioAvgViews = (decimal)video.ViewCount / (decimal)channel.AvgViews;
                await _videoRepository.Update(video);
            }
        }

        return true;
    }
}