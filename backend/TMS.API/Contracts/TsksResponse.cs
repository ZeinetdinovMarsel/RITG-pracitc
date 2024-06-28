namespace TMS.API.Contracts
{
    public record TsksResponse(
        Guid Id,
        Guid CreatorId,
        Guid AssignedUserId,
        string Title,
        string Comment,
        string Priority,
        int Status,
        DateTime StartDate,
        DateTime EndDate
    );
    public record TsksRequest(
        Guid AssignedUserId,
        string Title,
        string Comment,
        string Priority,
        int Status,
        DateTime StartDate,
        DateTime EndDate
    );
}
