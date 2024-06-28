using TMS.Core.Enums;

namespace TMS.Core.Models
{
    public class Tsk
    {
        public const int MAX_TITLE_LENGTH = 50;
        private Tsk(Guid id, Guid creatorId, Guid assignedUserId,
            string title, string comment,
            string priority, int status,
            DateTime startDate, DateTime endDate)
        {
            Id = id;
            CreatorId = creatorId;
            Title = title;
            Comment = comment;
            AssignedUserId = assignedUserId;
            Priority = priority;
            Status = status;
            StartDate = startDate;
            EndDate = endDate;
        }
        public Guid Id { get; }
        public Guid CreatorId { get; } = Guid.Empty;
        public Guid AssignedUserId { get; } = Guid.Empty;
        public string Title { get; } = string.Empty;
        public string Comment { get; } = string.Empty;
        public string Priority { get; } = string.Empty;
        public int Status { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public static (Tsk Tsk, string Error) Create(
            Guid id, Guid creatorId, Guid assignedUserId,
            string title, string comment,
            string priority, int status,
            DateTime startDate, DateTime endDate)
        {
            var error = string.Empty;
            if (string.IsNullOrEmpty(title) || title.Length > MAX_TITLE_LENGTH)
            {
                error = $"Название не может быть пустым или длиннее чем {MAX_TITLE_LENGTH} символов";
            }
            else if (startDate >= endDate)
            {
                error = "Дата конца не может быть раньше или равна дате начала";
            }
            var Tsk = new Tsk(id, creatorId, assignedUserId, title, comment, priority, status, startDate, endDate);
            return (Tsk, error);
        }

    }
}
