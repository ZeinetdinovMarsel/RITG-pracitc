namespace TMS.API.Contracts
{
    public record TsksResponse(
        Guid Id,
        string Title,
        string Comment,
        string AssignedUserId,
        string Priority,
        string Status,
        DateTime StartDate,
        DateTime EndDate
    );
    public record TsksRequest(
        string Title,
        string Comment,
        string AssignedUserId,
        string Priority,
        string Status,
        DateTime StartDate,
        DateTime EndDate
    );
}
