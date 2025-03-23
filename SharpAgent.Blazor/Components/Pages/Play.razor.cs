using MediatR;

using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

using SharpAgent.Application.Channels.Common;
using SharpAgent.Application.Videos.Commands.UpdateNotes;
using SharpAgent.Application.Videos.Common;
using SharpAgent.Application.Videos.Queries.GetById;
using SharpAgent.Application.YouTube.Commands.SaveVideos;
using SharpAgent.Domain.Entities;

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

    private string updateMessage;
    private VideoResponse video = new();
    private string note = string.Empty;

    private bool hasTranscript = false;
    private string transcriptText = string.Empty;

    private bool hasSummary = false;
    private string summaryText = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        if (Id != Guid.Empty)
        {
            await GetVideo(Id);
            await CheckForExistingSummary();
        }
        else
        {
            video = new();
        }
    }

    private async Task CheckForExistingSummary()
    {
        try
        {
            var getVideoSummaryQuery = new SharpAgent.Application.AiSummaries.Queries.GetMostRecent.GetMostRecentAiSummaryQuery
            {
                VideoId = video.Id
            };

            var aiSummary = await Mediator.Send(getVideoSummaryQuery);

            if (aiSummary != null)
            {
                if (!aiSummary.Transcript.IsNullOrEmpty())
                {
                    transcriptText = aiSummary.Transcript;
                    hasTranscript = true;
                }

                if (!aiSummary.Summary.IsNullOrEmpty())
                {
                    summaryText = aiSummary.Summary;
                    hasSummary = true;
                }

                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error checking for existing summary: {ex.Message}");
        }
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

    private async Task RetrieveTranscript()
    {
        try
        {
            updateMessage = "Retrieving transcript...";
            StateHasChanged();

            var command = new SharpAgent.Application.Videos.Commands.RetrieveTranscript.RetrieveVideoTranscriptCommand
            {
                VideoId = video.Id
            };

            var summaryId = await Mediator.Send(command);

            if (summaryId.HasValue)
            {
                updateMessage = "Transcript retrieved successfully!";
                await CheckForExistingSummary();
            }
            else
            {
                updateMessage = "Failed to retrieve transcript.";
            }
        }
        catch (Exception ex)
        {
            updateMessage = $"Error retrieving transcript: {ex.Message}";
            Console.Error.WriteLine($"Error in RetrieveTranscript: {ex.Message}");
        }
        finally
        {
            StateHasChanged();
        }
    }

    private async Task CreateSummary()
    {
        try
        {
            updateMessage = "Creating summary...";
            StateHasChanged();

            // First, check if a summary exists or needs to be created
            var getVideoSummaryCommand = new SharpAgent.Application.Videos.Commands.GetSummary.GetVideoSummaryCommand
            {
                VideoId = video.Id
            };

            // This will either retrieve an existing summary or create a new one
            var summary = await Mediator.Send(getVideoSummaryCommand);

            if (summary != null)
            {
                updateMessage = "Summary retrieved successfully!";
                // Store the summary data to display in the UI
                summaryText = summary.Summary;
                hasTranscript = !string.IsNullOrEmpty(summary.Transcript);

                // For full implementation, you might want to update the UI state or navigate
                StateHasChanged();
            }
            else
            {
                updateMessage = "Failed to create or retrieve summary.";
            }
        }
        catch (Exception ex)
        {
            updateMessage = $"Error creating/retrieving summary: {ex.Message}";
            Console.Error.WriteLine($"Error in CreateSummary: {ex.Message}");
        }
        finally
        {
            StateHasChanged();
        }
    }
}
