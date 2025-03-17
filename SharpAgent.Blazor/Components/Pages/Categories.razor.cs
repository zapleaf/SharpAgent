using System.Runtime.CompilerServices;

using MediatR;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

using SharpAgent.Application.Channels.Common;
using SharpAgent.Application.Channels.Queries.GetByCategory;
using SharpAgent.Application.Channels.Commands.UpdateStats;
using SharpAgent.Application.Categories.Commands.Create;
using SharpAgent.Application.Categories.Commands.Delete;
using SharpAgent.Application.Categories.Queries.GetAll;
using SharpAgent.Application.Videos.Queries.GetByCategory;
using SharpAgent.Application.Videos.Common;


namespace SharpAgent.Blazor.Components.Pages;

public partial class Categories
{
    [Inject]
    protected IMediator Mediator { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    protected List<CategoryResponse> categories = new();
    protected CreateCategoryRequest newCategory = new();
    protected CategoryResponse currentCategory = new();
    protected bool Loading { get; set; }

    private List<VideoResponse> Videos { get; set; } = [];
    private List<VideoResponse> AllVideos { get; set; } = [];
    private List<ChannelResponse> Channels { get; set; } = [];

    private int VideoCount = 0;

    private bool watchedOnly = false;  // Filters by watched only or all videos
    private string orderby = "newest"; // Filters by newest or number of views
    private string view = "videos";    // Displays videos or channels

    protected override async Task OnInitializedAsync()
    {
        await LoadCategories();
    }

    protected async Task LoadCategories()
    {
        Loading = true;
        categories = await Mediator.Send(new GetAllCategoriesQuery());
        Loading = false;
        StateHasChanged();
    }

    protected async Task AddCategory()
    {
        try
        {
            Loading = true;
            var command = new CreateCategoryCommand
            {
                Category = new CreateCategoryRequest
                {
                    Name = newCategory.Name,
                    Description = newCategory.Description
                }
            };

            var result = await Mediator.Send(command);
            if (result != null)
            {
                newCategory = new();
                await LoadCategories();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating category: {ex.Message}");
        }
        finally
        {
            Loading = false;
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

    private async void ViewChannels()
    {
        view = "channels";
        await LoadChannels();
    }

    private void ViewVideos()
    {
        view = "videos";
        OrganizeVideos();
    }

    private void ViewOutliers()
    {
        view = "outliers";
        OrganizeVideos();
    }

    private void OrganizeVideos()
    {
        Loading = true;
        if (watchedOnly)
            Videos = AllVideos.Where(x => x.WasWatched = watchedOnly).ToList();
        else if (view == "outliers")
            Videos = AllVideos.Where(x => x.RatioAvgViews > 1.2m).ToList();
        else
            Videos = AllVideos;

        if (orderby == "views")
            Videos = Videos.OrderByDescending(x => x.ViewCount).ToList();
        else
            Videos = Videos.OrderByDescending(x => x.PublishedAt).ToList();

        Loading = false;
        StateHasChanged();
    }

    protected async Task DeleteCategory(Guid id)
    {
        try
        {
            Loading = true;
            var command = new DeleteCategoryCommand { Id = id };
            var result = await Mediator.Send(command);
            if (result)
            {
                await LoadCategories();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting category: {ex.Message}");
        }
        finally
        {
            Loading = false;
        }
    }

    private async void GetVideos(CategoryResponse category)
    {
        Loading = true;

        currentCategory = category;

        var query = new GetVideosByCategoryQuery { CategoryId = currentCategory.Id };
        AllVideos = await Mediator.Send(query);

        OrganizeVideos();
        Loading = false;
        StateHasChanged();
    }

    private async Task LoadChannels()
    {
        Loading = true;

        var query = new GetChannelsByCategoryQuery { CategoryId = currentCategory.Id };
        Channels = await Mediator.Send(query);

        OrganizeVideos();
        Loading = false;
        StateHasChanged();
    }

    private async Task UpdateStats(Guid channelId)
    {
        Loading = true;
        var command = new UpdateChannelStatsCommand { ChannelId = channelId };
        await Mediator.Send(command);
        await LoadChannels();
        Loading = false;
        StateHasChanged();
    }
}
