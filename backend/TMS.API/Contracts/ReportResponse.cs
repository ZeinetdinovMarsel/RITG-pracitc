using TMS.Core.Models;

namespace TMS.API.Contracts
{
    public record ReportResponse
    (
        DateTime Date,
        int TotalTasks,
        int NewTasks,
        int InProgressTasks,
        int CompletedTasks,
       int HighPriorityTasks,
       int MediumPriorityTasks,
       int LowPriorityTasks,
       int OverdueTasks,
       double AverageCompletionTime
    );
}
