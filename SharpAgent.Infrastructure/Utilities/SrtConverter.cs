using System.Text;
using System.Text.RegularExpressions;

namespace SharpAgent.Infrastructure.Utilities;

public class SrtConverter
{
    public static string ConvertSrtToSimpleFormat(string srtContent)
    {
        // Prepare output StringBuilder
        StringBuilder simpleOutput = new StringBuilder();

        // Split the input by double newlines to get each subtitle entry
        string[] entries = srtContent.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string entry in entries)
        {
            // Split each entry into lines
            string[] lines = entry.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Need at least 2 lines (sequence number and timestamp)
            if (lines.Length < 2)
                continue;

            // Parse the timestamp line (should be the second line)
            string timestampLine = lines[1];

            // Extract the start time using regex
            Match match = Regex.Match(timestampLine, @"(\d+):(\d+):(\d+),\d+");

            if (!match.Success)
                continue;

            // Get the hours, minutes, and seconds as separate groups and pad if needed
            string hours = match.Groups[1].Value.PadLeft(2, '0');
            string minutes = match.Groups[2].Value.PadLeft(2, '0');
            string seconds = match.Groups[3].Value.PadLeft(2, '0');

            // Format the time with padded values
            string formattedTime = $"{hours}:{minutes}:{seconds}";

            // Get the subtitle text (can be multiple lines)
            StringBuilder textBuilder = new StringBuilder();
            for (int i = 2; i < lines.Length; i++)
            {
                // Skip empty lines or lines with just spaces
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                textBuilder.Append(lines[i].Trim() + " ");
            }

            string subtitleText = textBuilder.ToString().Trim();

            // Skip entries that only contain spaces or empty text
            if (string.IsNullOrWhiteSpace(subtitleText))
                continue;

            // Add to output in the format StartTime: Phrase (with space after colon)
            simpleOutput.AppendLine($"{formattedTime}  {subtitleText}");
        }

        return simpleOutput.ToString();
    }
}