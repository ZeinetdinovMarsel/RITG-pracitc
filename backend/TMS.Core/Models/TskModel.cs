public class TaskModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public Guid AssignedUserId { get; set; }
    public string Title { get; set; }
    public string Comment { get; set; }
    public int Priority { get; set; }
    public int Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime AcceptDate { get; set; }
    public DateTime FinishDate { get; set; }
}