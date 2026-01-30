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
        public bool IsSuccess => Status == ResultStatus.Success;
        public ResultStatus Status { get; }
        public string Message { get; }
        public T Data { get; }

        public static OperationResult<T> Ok(T data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            return new OperationResult<T>(ResultStatus.Success, "Ok", data);
        }
        public static OperationResult<T> Fail(ResultStatus status = ResultStatus.Fail, string message = "Fail")
        {
            return new OperationResult<T>(status, message, default!);
        }
        private OperationResult(ResultStatus status, string message, T data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
