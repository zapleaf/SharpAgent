using Microsoft.AspNetCore.Components;

using MediatR;

using SharpAgent.Application.Channels.Common;
using SharpAgent.Application.Channels.Commands.UpdateNotes;
using SharpAgent.Application.Channels.Commands.Delete;
using SharpAgent.Application.Channels.Commands.AddCategory;
using SharpAgent.Application.Channels.Commands.RemoveCategory;
using SharpAgent.Application.Channels.Commands.UpdateStats;
using SharpAgent.Application.Channels.Queries.GetAll;
using SharpAgent.Application.Channels.Queries.GetById;

using SharpAgent.Application.Videos.Common;
using SharpAgent.Application.Videos.Queries.GetByChannel;

using SharpAgent.Application.YouTube.Commands.SaveVideos;

using SharpAgent.Application.Categories.Queries.GetAll;

using SharpAgent.Infrastructure.Services;

namespace SharpAgent.Blazor.Components.Pages;

public partial class Channels
{
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Parameter] public Guid Id { get; set; }
    [Parameter] public string View { get; set; }

    private bool isDisabled = true;
    private bool showModal { get; set; } = false;
    private List<ChannelResponse> channels = new();
    private ChannelResponse? currentChannel;
    private List<VideoResponse> videos = new();
    private List<VideoResponse> allVideos = new();
    private string outlierVideos;
    private string averageVideos;
    private List<CategoryResponse> categories = new List<CategoryResponse>();

    private bool loading;
    private string updateMessage = string.Empty;
    private bool watchedOnly;
    private string orderby = "newest";

    protected override async Task OnInitializedAsync()
    {
        View ??= "videos";
        await GetCategories();
        await GetChannels();
        if (Id != Guid.Empty)
        {
            var channel = channels.FirstOrDefault(c => c.Id == Id);
            if (channel != null)
                await GetVideos(Id);
        }
    }

    private async Task GetChannels()
    {
        loading = true;

        var query = new GetAllChannelsQuery
        {
            IncludeVideos = true,
            IncludeCategories = true
        };

        channels = await Mediator.Send(query);

        loading = false;
        StateHasChanged();
    }

    private async Task GetVideos(Guid channelId)
    {
        loading = true;
        updateMessage = string.Empty;

        currentChannel = channels.FirstOrDefault(c => c.Id == channelId);

        var query = new GetVideosByChannelQuery { ChannelId = channelId };
        allVideos = await Mediator.Send(query);

        OrganizeVideos();
        loading = false;
        StateHasChanged(); // Explicitly request the UI to update
    }

    private async Task GetCategories()
    {
        loading = true;

        var query = new GetAllCategoriesQuery();
        categories = await Mediator.Send(query);
        categories = categories.OrderBy(c => c.Name).ToList();

        loading = false;
        StateHasChanged(); // Explicitly request the UI to update
    }

    private async Task UpdateVideos(ChannelResponse? channel)
    {
        if (channel == null) return;

        loading = true;

        var command = new SaveChannelVideosCommand
        {
            ChannelYTId = channel.YTId,
            ChannelId = channel.Id,
            LastCheckDate = channel.LastCheckDate
        };

        var updatedCount = await Mediator.Send(command);
        updateMessage = $"{updatedCount} added";

        await GetVideos(channel.Id);
        loading = false;
    }

    private async Task DeleteChannel(Guid channelId)
    {
        loading = true;

        try
        {
            var command = new DeleteChannelCommand { ChannelId = channelId };
            var result = await Mediator.Send(command);

            if (result)
            {
                var channel = channels.FirstOrDefault(c => c.Id == channelId);
                if (channel != null)
                {
                    channels.Remove(channel);
                    currentChannel = null;
                    videos.Clear();
                    allVideos.Clear();
                }
            }
        }
        catch (Exception ex)
        {
            // TODO: Add error handling
            Console.WriteLine($"Error deleting channel: {ex.Message}");
        }
        finally
        {
            loading = false;
            StateHasChanged();
        }
    }

    private void ViewAll()
    {
        watchedOnly = false;
        OrganizeVideos();
    }

    private void FilterByWatched()
    {
        watchedOnly = true;
        OrganizeVideos();
    }

    private void OrderByNewest()
    {
        orderby = "newest";
        OrganizeVideos();
    }

    private void OrderByViews()
    {
        orderby = "views";
        OrganizeVideos();
    }

    private void ViewVideos()
    {
        View = "videos";
        OrganizeVideos();
    }

    private void ViewDetails()
    {
        View = "details";
    }

    private void ViewOutliers()
    {
        View = "outliers";
        OrganizeVideos();
    }

    private void OrganizeVideos()
    {
        loading = true;

        videos = allVideos.ToList();

        if (watchedOnly)
            videos = videos.Where(x => !x.WasWatched).ToList();
        else if (View == "outliers")
            videos = videos.Where(x => x.RatioAvgViews > 1.2m).ToList();

        videos = orderby switch
        {
            "views" => videos.OrderByDescending(x => x.ViewCount).ToList(),
            _ => videos.OrderByDescending(x => x.PublishedAt).ToList()
        };

        loading = false;
        StateHasChanged();
    }

    private async Task RemoveCategoryFromChannel(Guid categoryId, Guid channelId)
    {
        var command = new RemoveCategoryFromChannelCommand
        {
            CategoryId = categoryId,
            ChannelId = channelId
        };
        await Mediator.Send(command);
        await UpdateChannel(channelId);
    }

    private async Task AddCategoryToChannel(Guid categoryId, Guid channelId)
    {
        var command = new AddCategoryToChannelCommand
        {
            CategoryId = categoryId,
            ChannelId = channelId
        };
        await Mediator.Send(command);
        await UpdateChannel(channelId);
    }

    private async Task UpdateChannel(Guid channelId)
    {
        loading = true;

        var query = new GetChannelByIdQuery(channelId);
        var channelDto = await Mediator.Send(query);

        var index = channels.FindIndex(c => c.Id == channelId);
        if (index != -1)
        {
            channels[index] = channelDto;
            currentChannel = channelDto;
        }

        loading = false;
        StateHasChanged();
    }

    private void ToggleEnabled()
    {
        isDisabled = !isDisabled;
    }

    private async Task UpdateStats(Guid channelId)
    {
        loading = true;
        var command = new UpdateChannelStatsCommand { ChannelId = channelId };
        await Mediator.Send(command);
        await GetChannels();
        loading = false;
        StateHasChanged();
    }

    private async Task UpdateNotes(ChangeEventArgs e)
    {
        loading = true;

        if (currentChannel == null) return;

        var command = new UpdateChannelNotesCommand
        {
            ChannelId = currentChannel.Id,
            Notes = currentChannel.Notes ?? string.Empty
        };
        await Mediator.Send(command);

        loading = false;
        StateHasChanged();
    }

    private void ShowModal()
    {
        showModal = true;
        outlierVideos = string.Join(", ",
            videos.Where(x => x.RatioAvgViews > 1m)
                  .Select(video => $"{video.Title} - {video.ViewCount} Views"));
        averageVideos = string.Join(", ",
            videos.Where(x => x.RatioAvgViews <= 1m)
                  .Select(video => $"{video.Title} - {video.ViewCount} Views"));
        StateHasChanged();
    }

    private void CloseModal()
    {
        showModal = false;
    }
}
