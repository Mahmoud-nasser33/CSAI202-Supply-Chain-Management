using Microsoft.Data.SqlClient;

namespace SilsilaSupply.Services
{
    public static class SqlReaderExtensions
    {
        public static string? GetStringOrNull(this SqlDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
        }

        public static int? GetInt32OrNull(this SqlDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? null : reader.GetInt32(ordinal);
        }

        public static decimal? GetDecimalOrNull(this SqlDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? null : reader.GetDecimal(ordinal);
        }

        public static DateTime? GetDateTimeOrNull(this SqlDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? null : reader.GetDateTime(ordinal);
        }
    }
}
