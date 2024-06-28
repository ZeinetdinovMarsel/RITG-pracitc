namespace TMS.DataAccess.Entities
{
    public class TskEntity
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; } = Guid.Empty;
        public Guid AssignedUserId { get; set; } = Guid.Empty;
        public string Title { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public int Status { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
