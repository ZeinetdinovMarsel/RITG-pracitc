using TMS.Core.Enums;

namespace TMS.Core.Models
{
    public class Tsk
    {
        public const int MAX_TITLE_LENGTH = 50;
        private Tsk(Guid id, Guid creatorId, Guid assignedUserId,
            string title, string comment,
            int priority, int status,
            DateTime startDate, DateTime endDate,
            DateTime acceptDate, DateTime finishDate)
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
            AcceptDate = acceptDate;
            FinishDate = finishDate;

        }
        public Guid Id { get; }
        public Guid CreatorId { get; } = Guid.Empty;
        public Guid AssignedUserId { get; } = Guid.Empty;
        public string Title { get; } = string.Empty;
        public string Comment { get; } = string.Empty;
        public int Priority { get; }
        public int Status { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public DateTime AcceptDate { get; }
        public DateTime FinishDate { get; }
        public static (Tsk Tsk, string Error) Create(TaskModel tsk)
        {
            var error = string.Empty;
            if (string.IsNullOrEmpty(tsk.Title) || tsk.Title.Length > MAX_TITLE_LENGTH)
            {
                error = $"Название не может быть пустым или длиннее чем {MAX_TITLE_LENGTH} символов";
            }
            else if (tsk.StartDate >= tsk.EndDate)
            {
                error = "Дата конца не может быть раньше или равна дате начала";
            }

            var Tsk = new Tsk(
                tsk.Id,
                tsk.CreatorId, 
                tsk.AssignedUserId, 
                tsk.Title, 
                tsk.Comment, 
                tsk.Priority, 
                tsk.Status, 
                tsk.StartDate, 
                tsk.EndDate, 
                tsk.AcceptDate, 
                tsk.FinishDate);

            return (Tsk, error);
        }

    }
}
