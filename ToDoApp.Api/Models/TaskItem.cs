using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Api.Models
{
    public class TaskItem
    {
        public int TaskItemId { get; set; }
        public Guid UserId { get; set; }
        
        [MaxLength(1000)]
        public string TaskDescription { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
