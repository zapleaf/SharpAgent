using AutoMapper;

using Google.Apis.Services;
using Google.Apis.YouTube.v3;

using System.Text.RegularExpressions;
//using System.Threading.Channels;

using SharpAgent.Domain.Common;
using SharpAgent.Application.Channels.Common;
using SharpAgent.Application.IRepositories;
using SharpAgent.Application.IServices;

using SharpAgent.Domain.Entities;

namespace SharpAgent.Infrastructure.Services;

public class YouTubeApiService : IYouTubeApiService
{
    private readonly YouTubeService _youTubeService;
    private readonly IMapper _mapper;

    public YouTubeApiService(YouTubeApiKey youTubeApiKey, IMapper mapper)
    {
        _mapper = mapper;

        _youTubeService = new YouTubeService(new BaseClientService.Initializer()
        {
            ApiKey = youTubeApiKey.Key,
            ApplicationName = this.GetType().ToString()
        });
    }

    public async Task<List<ChannelResponse>> ChannelSearch(string searchTerm)
    {
        var searchListRequest = _youTubeService.Search.List("snippet");
        searchListRequest.Q = searchTerm;
        searchListRequest.Type = "channel";
        searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Relevance;
        searchListRequest.MaxResults = 10;

        var searchListResponse = await searchListRequest.ExecuteAsync();
        var channels = new List<Channel>();

        foreach (var searchResult in searchListResponse.Items)
        {
            var channel = new Channel
            {
                ThumbnailURL = searchResult.Snippet.Thumbnails.Default__.Url,
                YTId = searchResult.Snippet.ChannelId,
                Title = searchResult.Snippet.Title,
                Description = searchResult.Snippet.Description,
            };

            if (DateTime.TryParse(searchResult.Snippet.PublishedAtRaw, out DateTime publishedAt))
            {
                channel.PublishedAt = publishedAt;
            }
            channels.Add(channel);
        }

        return _mapper.Map<List<ChannelResponse>>(channels);
    }

    public async Task<List<Video>> VideoSearch(string searchTerm, DateTime? startDate = null)
    {
        var searchListRequest = _youTubeService.Search.List("snippet");
        searchListRequest.Q = searchTerm;
        searchListRequest.Type = "video";
        searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Relevance;
        searchListRequest.MaxResults = 50;

        if (startDate.HasValue)
        {
            searchListRequest.PublishedAfter = startDate.Value;
        }

        var searchListResponse = await searchListRequest.ExecuteAsync();
        var videos = new List<Video>();

        foreach (var searchResult in searchListResponse.Items)
        {
            var video = new Video
            {
                ThumbnailUrl = searchResult.Snippet.Thumbnails.Medium.Url,
                YTChannelId = searchResult.Snippet.ChannelId,
                YTId = searchResult.Id.VideoId,
                Title = searchResult.Snippet.Title,
                Description = searchResult.Snippet.Description
            };

            if (DateTime.TryParse(searchResult.Snippet.PublishedAtRaw, out DateTime publishedAt))
            {
                video.PublishedAt = publishedAt;
            }

            videos.Add(video);
        }

        return videos;
    }

    public async Task<Channel> GetChannelStats(string youTubeId)
    {
        var channel = new Channel { YTId = youTubeId };
        var channelRequest = _youTubeService.Channels.List("statistics");
        channelRequest.Id = youTubeId;

        var channelResponse = await channelRequest.ExecuteAsync();
        if (channelResponse.Items.Count > 0)
        {
            var channelItem = channelResponse.Items[0];
            channel.SubscriberCount = channelItem.Statistics.SubscriberCount;
            channel.VideoCount = channelItem.Statistics.VideoCount;
        }

        return channel;
    }

    public async Task<Video> GetStats(Video video)
    {
        var videoRequest = _youTubeService.Videos.List("statistics");
        videoRequest.Id = video.YTId;

        var response = await videoRequest.ExecuteAsync();
        if (response.Items?.Count > 0)
        {
            var stats = response.Items[0].Statistics;
            video.ViewCount = (int)(stats.ViewCount ?? 0);
            video.LikeCount = (int)(stats.LikeCount ?? 0);
            video.CommentCount = (int)(stats.CommentCount ?? 0);
        }

        return video;
    }

    public async Task<int?> GetDuration(string youTubeId)
    {
        var videoRequest = _youTubeService.Videos.List("contentDetails");
        videoRequest.Id = youTubeId;

        var videoResponse = await videoRequest.ExecuteAsync();
        if (videoResponse.Items.Count > 0)
        {
            return ConvertISO8601ToSeconds(videoResponse.Items[0].ContentDetails.Duration);
        }

        return null;
    }

    public async Task<List<Video>> GetChannelVideos(string channelId, DateTime? since = null)
    {
        var searchListRequest = _youTubeService.Search.List("snippet");
        searchListRequest.ChannelId = channelId;
        searchListRequest.MaxResults = 25;
        searchListRequest.Type = "video";

        if (since != null)
            searchListRequest.PublishedAfterDateTimeOffset = since;

        var results = new List<Google.Apis.YouTube.v3.Data.SearchResult>();
        string? nextPageToken = null;

        do
        {
            searchListRequest.PageToken = nextPageToken;
            var searchListResponse = await searchListRequest.ExecuteAsync();
            results.AddRange(searchListResponse.Items);
            nextPageToken = searchListResponse.NextPageToken;
        } while (nextPageToken != null);

        return results.Select(result => new Video
        {
            ThumbnailUrl = result.Snippet.Thumbnails.Medium.Url,
            YTChannelId = result.Snippet.ChannelId,
            YTId = result.Id.VideoId,
            Title = result.Snippet.Title,
            Description = result.Snippet.Description,
            PublishedAt = DateTime.TryParse(result.Snippet.PublishedAtRaw, out DateTime publishedAt) ? publishedAt : null
        }).ToList();
    }

    private static int ConvertISO8601ToSeconds(string iso8601Duration)
    {
        var match = Regex.Match(iso8601Duration, @"PT(?:(\d+)H)?(?:(\d+)M)?(?:(\d+)S)?");

        if (!match.Success) return 0;

        int hours = match.Groups[1].Value != "" ? int.Parse(match.Groups[1].Value) : 0;
        int minutes = match.Groups[2].Value != "" ? int.Parse(match.Groups[2].Value) : 0;
        int seconds = match.Groups[3].Value != "" ? int.Parse(match.Groups[3].Value) : 0;

        return hours * 3600 + minutes * 60 + seconds;
    }
}
