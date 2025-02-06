namespace Finances.Core.Contexts.SharedContext.Entities;

public class Response
{
    public int StatusCode { get; set; }

    public bool Success { get; set; }

    public object Data { get; set; }

    public string Message { get; set; }

    public Response(int statusCode, bool success, object data = null, string message = null)
    {
        this.StatusCode = statusCode;
        this.Success = success;
        this.Data = data;
        this.Message = message;
    }
}

public class ValidationErrorResponse : Response
{
    public const string DefaultMessageError = "Validation errors...";

    public ValidationErrorResponse(int statusCode, bool success, object data = null, string message = null) : base(statusCode, success, data, message) { }

    public static ValidationErrorResponse CreateError(int statusCode, object data = null, string message = DefaultMessageError) => new(statusCode, false, data, message);
}

public class GlobalExceptionHandlerResponse : Response
{
    public const string DefaultMessageError = "Ops... an error has occurred, contact developers team.";

    public GlobalExceptionHandlerResponse(int statusCode, bool success, object data = null, string message = null) : base(statusCode, success, data, message) { }

    public static GlobalExceptionHandlerResponse CreateError(int statusCode, object data = null, string message = DefaultMessageError) => new(statusCode, false, data, message);
}