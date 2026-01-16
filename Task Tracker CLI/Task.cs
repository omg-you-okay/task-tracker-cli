namespace Task_Tracker_CLI;

public enum Status
{
  Unknown = 0,
  Todo = 1,
  InProgress = 2,
  Done = 3
}

public class Task
{
  public required string Id { get; set; }
  public required string Description { get; set; }
  public Status Status { get; set; } = Status.Todo;
  public DateTime? CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}