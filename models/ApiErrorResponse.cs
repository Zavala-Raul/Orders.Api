namespace Orders.Api.models
{
    public sealed record ApiErrorResponse(
        int StatusCode,
        string Message,
        IReadOnlyList<FieldError> Errors);

    public sealed record FieldError(string Field, IReadOnlyList<string> Messages);
}