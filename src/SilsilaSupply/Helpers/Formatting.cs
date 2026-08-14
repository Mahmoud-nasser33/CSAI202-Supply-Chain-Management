namespace SilsilaSupply.Helpers
{
    public static class Formatting
    {
        public static string Ago(DateTime? when)
        {
            if (when is null)
                return "—";

            var span = DateTime.Now - when.Value;
            if (span.TotalMinutes < 1)
                return "just now";
            if (span.TotalMinutes < 60)
                return $"{(int)span.TotalMinutes} min ago";
            if (span.TotalHours < 24)
                return $"{(int)span.TotalHours} h ago";
            if (span.TotalDays < 7)
                return $"{(int)span.TotalDays} d ago";

            return when.Value.ToString("MMM d, yyyy");
        }
    }
}
