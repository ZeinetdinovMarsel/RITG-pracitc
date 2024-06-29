using System.Xml.Linq;
using TMS.Core.Enums;

namespace TMS.Core.Models
{
    public class TskHistory
    {
        private TskHistory(Guid id,Guid tskId,Guid userId,DateTime changeDate, string changes)
        {
            Id = id;
            TskId = tskId;
            UserId = userId;
            ChangeDate = changeDate;
            Changes = changes;
        }
        public Guid Id { get; }
        public Guid TskId { get; }
        public Guid UserId { get; }
        public DateTime ChangeDate { get; }
        public string Changes { get; } = string.Empty;

        public static TskHistory Create(Guid id, Guid tskId, Guid userId, DateTime changeDate, string changes)
        {
            var TskHistory = new TskHistory(id, tskId, userId, changeDate, changes);

            return TskHistory;
        }
    }

}
