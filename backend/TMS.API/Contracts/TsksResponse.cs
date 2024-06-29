namespace TMS.API.Contracts
{
    public record TsksHistoryResponse(
       Guid Id,
       Guid TskId,
       Guid UserId,
       DateTime ChangeDate,
       string Changes
   );

    public record TsksResponse(
        Guid Id,
        Guid CreatorId,
        Guid AssignedUserId,
        string Title,
        string Comment,
        int Priority,
        int Status,
        DateTime StartDate,
        DateTime EndDate
    );
    public record TsksRequest(
        Guid AssignedUserId,
        string Title,
        string Comment,
        int Priority,
        int Status,
        DateTime StartDate,
        DateTime EndDate
    );
}
