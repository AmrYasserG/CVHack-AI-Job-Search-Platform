using System.Collections.Generic;

namespace CVHack.Common;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();

    public static Result<T> Success(T data, string? message = null, int statusCode = 200) => new()
    {
        IsSuccess = true,
        StatusCode = statusCode,
        Message = message,
        Data = data
    };

    public static Result<T> Failure(List<string> errors, string? message = null, int statusCode = 400) => new()
    {
        IsSuccess = false,
        StatusCode = statusCode,
        Message = message,
        Errors = errors
    };

    public static Result<T> Failure(string error, string? message = null, int statusCode = 400) => new()
    {
        IsSuccess = false,
        StatusCode = statusCode,
        Message = message,
        Errors = new List<string> { error }
    };
}
