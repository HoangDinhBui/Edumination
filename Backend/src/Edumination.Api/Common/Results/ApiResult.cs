namespace Edumination.Api.Common.Results;

public record ApiResult<T>(bool Success, T? Data, string? Error);
