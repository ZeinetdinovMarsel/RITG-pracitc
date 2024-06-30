namespace TMS.DataAccess.Entities
{
    public class TskEntity
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; } = Guid.Empty;
        public Guid AssignedUserId { get; set; } = Guid.Empty;
        public string Title { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int Priority { get; set; }
        public int Status { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime AcceptDate { get; set; }
        public DateTime FinishDate { get; set; }
    }
}
