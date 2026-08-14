namespace SilsilaSupply.Services
{
    public sealed class OperationResult
    {
        public bool Success { get; }
        public string? ErrorMessage { get; }

        private OperationResult(bool success, string? errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public static OperationResult Ok() => new(true, null);
        public static OperationResult Fail(string errorMessage) => new(false, errorMessage);
    }

    public sealed class DataResult<T>
    {
        public bool Success { get; }
        public T Data { get; }
        public string? ErrorMessage { get; }

        private DataResult(bool success, T data, string? errorMessage)
        {
            Success = success;
            Data = data;
            ErrorMessage = errorMessage;
        }

        public static DataResult<T> Ok(T data) => new(true, data, null);
        public static DataResult<T> Fail(string errorMessage) => new(false, default!, errorMessage);
    }
}
