using SharpAgent.Domain.Entities;
using SharpAgent.Application.Channels.Common;

namespace SharpAgent.Application.IServices;

public interface IYouTubeApiService
{
    Task<List<ChannelResponse>> ChannelSearch(string searchTerm);
    Task<Channel> GetChannelStats(string youTubeId);
    Task<Video> GetStats(Video video);
    Task<int?> GetDuration(string youTubeId);
    Task<List<Video>> GetChannelVideos(string channelId, DateTime? since = null);
    Task<List<Video>> VideoSearch(string searchTerm, DateTime? startDate = null);
}
