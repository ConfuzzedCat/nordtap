using CSharpFunctionalExtensions;

namespace Shared.Data.DTO;

public class ResultDto
{
    public string Error { get; set; }
    public bool IsSuccess { get; set; }
    
    
    public static ResultDto ToDto(Result result)
    {
        return new ResultDto
        {
            IsSuccess = result.IsSuccess,
            Error = result.IsFailure ? result.Error : string.Empty,
        };
    }
}