namespace LibraryManager.Domain.Results
{
    public enum ResultStatus
    {
        Success,
        Fail,
        Duplicate,
        Ambiguous,
        InvalidIsbn,
        AuthorMissing,
        BookMissing,
        NotFound,
        InvalidRequest,
        Error,
    }

    public class OperationResult<T>
    {
        public ResultStatus Status { get; }
        public string Message { get; }
        public T? Data { get; }

        public static OperationResult<T> Ok(T data) => new OperationResult<T>(ResultStatus.Success, "Ok", data);
        public static OperationResult<T> NotFound(string message) => new OperationResult<T>(ResultStatus.NotFound, message);
        public static OperationResult<T> Fail(string message) => new OperationResult<T>(ResultStatus.Fail, message);
        public OperationResult(ResultStatus status, string message, T? data = default)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
