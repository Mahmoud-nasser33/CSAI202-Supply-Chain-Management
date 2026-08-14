namespace SilsilaSupply.Helpers
{
    public static class StatusBadges
    {
        public static string ClassFor(string? status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return "";

            var s = status.ToLowerInvariant();

            if (ContainsAny(s, "tam", "wasel", "delivered", "completed", "complete", "arrived", "done"))
                return "badge-ok";
            if (ContainsAny(s, "cancel", "failed", "rejected", "returned", "malfooda"))
                return "badge-danger";
            if (ContainsAny(s, "gari", "fe el-tareeq", "in transit", "in-transit", "transit", "processing", "progress", "shipped", "pending", "mo3alaq", "on hold"))
                return "badge-progress";

            return "";
        }

        private static bool ContainsAny(string value, params string[] terms)
        {
            foreach (var term in terms)
            {
                if (value.Contains(term))
                    return true;
            }
            return false;
        }
    }
}
