namespace SECrawler.Domain;

public class ApiResult
{
    public bool IsSuccessful { get; set; }

    public string? ErrorMessage { get; set; } = null!;

    public static ApiResult Success()
    {
        return new ApiResult { IsSuccessful = true };
    }

    public static ApiResult Fail(string errorMessage)
    {
        return new ApiResult { IsSuccessful = false, ErrorMessage = errorMessage };
    }
}

public class ApiResult<T> : ApiResult
{
    public T? Data { get; set; }

    public static ApiResult<T> Fail(string? errorMessage)
    {
        return new ApiResult<T>() { IsSuccessful = false, ErrorMessage = errorMessage };
    }

    public static ApiResult<T> Success(T data)
    {
        return new ApiResult<T>() { Data = data, IsSuccessful = true };
    }
}