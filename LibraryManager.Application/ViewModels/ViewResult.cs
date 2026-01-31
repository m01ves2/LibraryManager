namespace LibraryManager.Application.ViewModels
{
    public enum ViewResultStatus
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
    public class ViewResult<T>
    {
        public bool IsSuccess => Status == ViewResultStatus.Success;
        public ViewResultStatus Status { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
