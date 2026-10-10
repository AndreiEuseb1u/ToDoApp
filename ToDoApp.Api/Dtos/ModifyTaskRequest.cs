namespace ToDoApp.Api.Dtos
{
    // PUT replaces the whole task: all fields must be sent. A missing field takes the
    // type's default (e.g. IsCompleted = false), it does not keep its stored value.
    public record ModifyTaskRequest(string TaskDescription, bool IsCompleted, uint RowVersion);
}
