namespace LibraryManager.Application.Results
{
    public enum ResultStatus
    {
        Success,// — всё ок
        NotFound,// — сущность не найдена
        InvalidInput,// — ISBN, пустые поля, доменная валидация
        Conflict,// — дубликаты, неоднозначность
        Error,// — непредвиденная ошибка (exception)
    }

    public class OperationResult<T>
    {
        public ResultStatus Status { get; }
        public string Message { get; }
        public T? Data { get; }

        public static OperationResult<T> Ok(T data) => new OperationResult<T>(ResultStatus.Success, "Ok", data);
        public static OperationResult<T> NotFound(string message) => new OperationResult<T>(ResultStatus.NotFound, message);
        public static OperationResult<T> InvalidInput(string message) => new OperationResult<T>(ResultStatus.InvalidInput, message);
        public static OperationResult<T> Conflict(string message) => new OperationResult<T>(ResultStatus.Conflict, message);
        public static OperationResult<T> Error(string message) => new OperationResult<T>(ResultStatus.Error, message);

        private OperationResult(ResultStatus status, string message, T? data = default)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
