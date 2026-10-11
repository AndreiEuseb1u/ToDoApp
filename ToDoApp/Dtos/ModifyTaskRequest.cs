namespace ToDoApp.Dtos
{
    public record ModifyTaskRequest(string TaskDescription, bool IsCompleted, uint RowVersion);
}
