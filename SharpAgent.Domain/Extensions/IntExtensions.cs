namespace SharpAgent.Domain.Extensions;

// Consider moving Extension methods to a shared project.

public static class IntExtensions
{
    public static string ToEstimateString(this int number)
    {
        if (number >= 1000 && number < 10000)
        {
            return (number / 1000.0).ToString("0.0") + "k";
        }
        else if (number >= 10000 && number < 1000000)
        {
            return (number / 1000).ToString() + "k";
        }
        else if (number >= 1000000)
        {
            return (number / 1000000.0).ToString("0.0") + "M";
        }
        else
        {
            return number.ToString();
        }
    }

    public static string ToEstimateString(this ulong? number)
    {
        ulong num = number ?? 0;

        if (num >= 1000 && num < 10000)
        {
            return (num / 1000.0).ToString("0.0") + "k";
        }
        else if (num >= 10000 && num < 1000000)
        {
            return (num / 1000).ToString() + "k";
        }
        else if (num >= 1000000)
        {
            return (num / 1000000.0).ToString("0.0") + "M";
        }
        else
        {
            return num.ToString();
        }
    }

    public static string ToMinuteString(this int? totalSeconds)
    {
        decimal seconds = totalSeconds ?? 0;
        // Convert seconds to minutes and round to the nearest whole number.
        // The Math.Round method is used here for rounding.
        int minutes = (int)Math.Round(seconds / 60);

        // Return the minutes in the desired format, e.g., "7m".
        return $"{minutes}m";
    }


    /// <summary>
    /// Formats seconds as a string HH:MM:SS
    /// </summary>
    public static string ToPrettyString(this int? totalSeconds)
    {
        int nonNull = totalSeconds ?? 0;

        int hours = nonNull / 3600;
        int minutes = (nonNull % 3600) / 60;
        int seconds = nonNull % 60;

        // Format the time as "HH:MM:SS"
        if (hours > 0)
            return $"{hours:00}:{minutes:00}:{seconds:00}";
        else
            return $"{minutes:00}:{seconds:00}";
    }
}

