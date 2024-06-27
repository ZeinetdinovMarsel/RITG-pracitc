namespace TMS.Core.Models
{
    public class Tsk
    {
        public const int MAX_TITLE_LENGTH = 250;
        private Tsk(Guid id, string title, string comment,
            string assignedUserId, string priority,
            string status, DateTime startDate, DateTime endDate)
        {
            Id = id;
            Title = title;
            Comment = comment;
            AssignedUserId = assignedUserId;
            Priority = priority;
            Status = status;
            StartDate = startDate;
            EndDate = endDate;
        }
        public Guid Id { get; }
        public string Title { get; } = string.Empty;
        public string Comment { get; } = string.Empty;
        public string AssignedUserId { get; }
        public string Priority { get; } = string.Empty;
        public string Status { get; } = string.Empty;
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public static (Tsk Tsk, string Error) Create(Guid id, string title, string comment,
            string assignedUserId, string priority,
            string status, DateTime startDate, DateTime endDate)
        {
            var error = string.Empty;
            if (string.IsNullOrEmpty(title) || title.Length > MAX_TITLE_LENGTH)
            {
                error = "Title can't be empty or longer than 250 characters";
            }
            var Tsk = new Tsk(id, title, comment, assignedUserId, priority, status, startDate, endDate);
            return (Tsk, error);
        }

    }
}
