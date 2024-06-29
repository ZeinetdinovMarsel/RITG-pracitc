namespace TMS.DataAccess.Entities
{
    public class TskHistoryEntity
    {
        public Guid Id { get; set; }
        public Guid TskId { get; set; }
        public Guid UserId { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Changes { get; set; } = string.Empty;
    }

}
