namespace Edumination.Api.Common.Results;

public class Result
{
    public bool Succeeded { get; init; }
    public string? Error { get; init; }
    public static Result Ok() => new() { Succeeded = true };
    public static Result Fail(string e) => new() { Succeeded = false, Error = e };
}

public class Result<T> : Result
{
    public T? Data { get; init; }
    public static Result<T> Ok(T data) => new() { Succeeded = true, Data = data };
    public new static Result<T> Fail(string e) => new() { Succeeded = false, Error = e };
}
