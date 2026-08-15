namespace ToDoApp.Api.Models
{
    public class TaskItem
    {
        public int TaskItemId { get; set; }
        public Guid UserId { get; set; }
        public string TaskDescription { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
