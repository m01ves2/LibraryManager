using LibraryManager.Application.ViewModels;
using LibraryManager.Domain.Results;

namespace LibraryManager.Application.Mappers
{
    public class ResultMapper
    {
        public static ViewResult<T> ToViewResult<T, S>(OperationResult<S> operationResult, Func<S, T> mapFunc)
        {
            return new ViewResult<T>()
            {
                Status = ResultStatusMapper.ToViewStatus(operationResult.Status),
                Message = operationResult.Message,
                Data = operationResult.Status == ResultStatus.Success ? mapFunc(operationResult.Data) : default!
            };
        }
    }
}
