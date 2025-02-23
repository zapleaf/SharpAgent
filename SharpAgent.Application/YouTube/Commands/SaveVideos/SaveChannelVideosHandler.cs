using SharpAgent.Application.IRepositories;
using SharpAgent.Application.IServices;
using SharpAgent.Domain.Entities;
using SharpAgent.Domain.Extensions;
using MediatR;

namespace SharpAgent.Application.YouTube.Commands.SaveVideos;

public class SaveChannelVideosHandler : IRequestHandler<SaveChannelVideosCommand, int>
{
    private readonly IVideoRepository _videoRepository;
    private readonly IYouTubeApiService _youTubeService;
    private readonly IChannelRepository _channelRepository;

    public SaveChannelVideosHandler(IVideoRepository videoRepository, IYouTubeApiService youTubeService, IChannelRepository channelRepository)
    {
        _videoRepository = videoRepository;
        _youTubeService = youTubeService;
        _channelRepository = channelRepository;
    }

    public async Task<int> Handle(SaveChannelVideosCommand request, CancellationToken cancellationToken)
    {
        // Get the current channel
        var currentChannel = await _channelRepository.Get(request.ChannelYTId);

        List<Video> videos = new();

        // If the LastCheckDate is null or minDate we assume its a new channel and we go back 1 year.
        if (!request.LastCheckDate.IsValidDate())
        {
            // We only want to go back 2 years
            request.LastCheckDate = DateTime.UtcNow.AddYears(-2);

            // Get videos from YouTube
            videos = await _youTubeService.GetChannelVideos(request.ChannelYTId, request.LastCheckDate);

            // If they have not released at least 4 videos in the last year we will skip
            var recentVidCount = videos.Where(v => v.PublishedAt < DateTime.UtcNow.AddYears(-1)).Count();
            if (recentVidCount < 5) return 0;
        }
        else
        {
            videos = await _youTubeService.GetChannelVideos(request.ChannelYTId, request.LastCheckDate);
        }

        // Process each video
        var validVideos = new List<Video>();
        foreach (var video in videos.Where(v => !v.Title.ToLower().Contains("#shorts")))
        {
            if (await _videoRepository.Get(video.YTId) != null) continue;
            if (validVideos.Any(v => v.YTId == video.YTId)) continue;

            video.Duration = await _youTubeService.GetDuration(video.YTId);

            // Only process videos within our target duration range
            if (video.Duration >= 65 && video.Duration <= 5400)
            {
                video.ChannelId = request.ChannelId;
                await _youTubeService.GetStats(video);
                validVideos.Add(video);

                // We are going to limit the number of videos to 50
                if (validVideos.Count >= 50) break;
            }
        }

        var addCount = await _videoRepository.Create(validVideos);

        currentChannel.LastCheckDate = DateTime.Now;
        await _channelRepository.Update(currentChannel);

        // Save valid videos to database
        return addCount;
    }
}
