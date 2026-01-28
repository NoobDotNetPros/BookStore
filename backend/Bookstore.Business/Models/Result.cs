namespace Bookstore.Business.Models;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T? Data { get; private set; }
    public string[] Errors { get; private set; } = Array.Empty<string>();

    private Result() { }

    public static Result<T> Success(T data) => new() { IsSuccess = true, Data = data };
    public static Result<T> Failure(params string[] errors) => new() { IsSuccess = false, Errors = errors };
}

public class Result
{
    public bool IsSuccess { get; private set; }
    public string[] Errors { get; private set; } = Array.Empty<string>();

    private Result() { }

    public static Result Success() => new() { IsSuccess = true };
    public static Result Failure(params string[] errors) => new() { IsSuccess = false, Errors = errors };
}
