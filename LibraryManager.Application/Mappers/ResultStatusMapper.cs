using LibraryManager.Application.ViewModels;
using LibraryManager.Domain.Results;
using System.Diagnostics;

namespace LibraryManager.Application.Mappers
{
    public class ResultStatusMapper
    {
        public static ViewResultStatus ToViewStatus(ResultStatus resultStatus)
            => resultStatus switch {
                ResultStatus.Success => ViewResultStatus.Success,
                ResultStatus.Fail => ViewResultStatus.Fail,
                ResultStatus.Duplicate => ViewResultStatus.Duplicate,
                ResultStatus.Ambiguous => ViewResultStatus.Ambiguous,
                ResultStatus.InvalidIsbn => ViewResultStatus.InvalidIsbn,
                ResultStatus.AuthorMissing => ViewResultStatus.AuthorMissing,
                ResultStatus.BookMissing => ViewResultStatus.BookMissing,
                ResultStatus.NotFound => ViewResultStatus.NotFound,
                ResultStatus.InvalidRequest => ViewResultStatus.InvalidRequest,
                ResultStatus.Error => ViewResultStatus.Error,
                _ => throw new ArgumentOutOfRangeException(),
            };
    }
}

