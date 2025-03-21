using MediatR;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using SharpAgent.Application.Channels.Common;
using SharpAgent.Application.Videos.Commands.UpdateNotes;
using SharpAgent.Application.Videos.Common;
using SharpAgent.Application.Videos.Queries.GetById;
using SharpAgent.Application.YouTube.Commands.SaveVideos;

namespace SharpAgent.Blazor.Components.Pages;

public partial class Play
{
    [Inject]
    protected IMediator Mediator { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Parameter]
    public Guid Id { get; set; }

    //private bool isDisabled = true;
    private string updateMessage;
    private VideoResponse video = new();
    private string note = string.Empty;

    //private int videoCount = 0;

    protected override async Task OnInitializedAsync()
    {
        if (Id != Guid.Empty)
        {
            await GetVideo(Id);
        }
        else
        {
            video = new();
        }
    }

    private async Task UpdateVideos(ChannelResponse channel)
    {
        var command = new SaveChannelVideosCommand
        {
            ChannelYTId = channel.YTId,
            ChannelId = channel.Id,
            LastCheckDate = channel.LastCheckDate
        };

        var updatedCount = await Mediator.Send(command);
        updateMessage = $"{updatedCount} added";
        StateHasChanged();
    }

    private async Task GetVideo(Guid Id)
    {
        var query = new GetVideoByIdQuery { Id = Id };
        video = await Mediator.Send(query);
        StateHasChanged();
    }

    private async Task AddNote()
    {
        try
        {
            // Get detailed player status for debugging
            var playerStatus = await JSRuntime.InvokeAsync<object>("isPlayerInitialized");
            Console.WriteLine($"Player status: {System.Text.Json.JsonSerializer.Serialize(playerStatus)}");

            var timestamp = await JSRuntime.InvokeAsync<double>("getCurrentTime");
            Console.WriteLine($"Retrieved timestamp: {timestamp}");

            // If timestamp is 0, it might be because the API failed
            if (timestamp <= 0)
            {
                // Try to manually get time from the DOM element if possible
                updateMessage = "Could not get exact timestamp. Adding note without precise timestamp.";
                // Continue with a generic note without timestamp or add a placeholder
                var stampedNote = $"(timestamp unavailable) - {note}";

                // Rest of your code to add the note
                // ...
            }
            else
            {
                var stampedNote = $"{TimeSpan.FromSeconds(timestamp):hh\\:mm\\:ss} - {note}";

                // Rest of your existing code
                var updatedNotes = string.IsNullOrEmpty(video.Notes)
                    ? stampedNote
                    : $"{video.Notes}\n{stampedNote}";

                var command = new UpdateVideoNotesCommand
                {
                    VideoId = video.Id,
                    Notes = updatedNotes
                };

                var success = await Mediator.Send(command);

                if (success)
                {
                    video.Notes = updatedNotes;
                    note = string.Empty;
                    updateMessage = "Note saved successfully!";
                }
                else
                {
                    updateMessage = "Failed to save note.";
                }
            }
        }
        catch (Exception ex)
        {
            updateMessage = $"An error occurred: {ex.Message}";
            Console.Error.WriteLine($"Error in AddNote: {ex.Message}");
        }
        finally
        {
            StateHasChanged();
        }
    }
}
