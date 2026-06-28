namespace Domain.DTOs;

public class ApiResponse<T>
{
    public T Data { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    
    public static ApiResponse<T> Success(T data) => new() { Data = data, IsSuccess = true };
    public static ApiResponse<T> Error(string errorMessage) => new() { IsSuccess = false, ErrorMessage = errorMessage };
}

public static class ApiResponse
{
    public static ApiResponse<T> Success<T>(T data) => ApiResponse<T>.Success(data);
    public static ApiResponse<T> Error<T>(string errorMessage) => ApiResponse<T>.Error(errorMessage);
}