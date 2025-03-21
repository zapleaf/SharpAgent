using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAgent.Domain.Models;

public class TranscriptResult
{
    public string VideoId { get; set; }
    public string Title { get; set; }
    public string ChannelId { get; set; }
    public string ChannelName { get; set; }
    public string ChannelUrl { get; set; }
    public long SubscriberCount { get; set; }
    public int ViewCount { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public string Duration { get; set; }
    public DateTime PublishedAt { get; set; }
    public string Description { get; set; }
    public string Subtitles { get; set; }
}
