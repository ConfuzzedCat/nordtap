using CSharpFunctionalExtensions;
using Shared.Data.DTO;

namespace Shared.Extensions;

public static class ResultExtensions
{
    public static ResultDto ToDto(this Result result)
    {
        return ResultDto.ToDto(result);
    }
}