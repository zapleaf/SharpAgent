namespace SharpAgent.Domain.Extensions;

public static class DateTimeExtensions
{
    public static bool IsValidDate(this DateTime? date)
    {
        return date.HasValue
            && date.Value != DateTime.MinValue
            && date.Value != DateTime.MaxValue;
    }

    public static string ToTimeAgo(this DateTime? date)
    {
        if (!date.HasValue)
        {
            return "Unknown"; // Or "N/A", or any other placeholder you prefer
        }

        DateTime now = DateTime.Now;
        TimeSpan timeSpan = now - date.Value;

        // Days
        if (timeSpan.Days < 14)
        {
            return $"{timeSpan.Days} {(timeSpan.Days == 1 ? "day ago" : "days ago")}";
        }

        // Weeks
        if (timeSpan.Days < 28) // 4 weeks * 7 days/week = 28 days
        {
            int weeks = timeSpan.Days / 7;
            return $"{weeks} {(weeks == 1 ? "week ago" : "weeks ago")}";
        }

        // Months
        if ((now.Year - date.Value.Year) * 12 + now.Month - date.Value.Month < 12)
        {
            int months = (now.Year - date.Value.Year) * 12 + now.Month - date.Value.Month;
            return $"{months} {(months == 1 ? "month ago" : "months ago")}";
        }

        // Years
        int years = now.Year - date.Value.Year;
        return $"{years} {(years == 1 ? "year ago" : "years ago")}";
    }
}
