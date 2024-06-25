namespace TMS.API.Contracts
{
    public record TsksResponse(
        Guid Id,
        string Title,
        string Description,
        int AssignedUserId,
        string Priority,
        string Status,
        DateTime StartDate,
        DateTime EndDate
    );
    public record TsksRequest(
        string Title,
        string Description,
        int AssignedUserId,
        string Priority,
        string Status,
        DateTime StartDate,
        DateTime EndDate
    );
}
