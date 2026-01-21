namespace Task_Tracker_CLI.Engines;

public interface ITaskEngine
{
  public string GenerateNextId(List<Task> existingTasks);
  public List<Task> FilterByStatus(List<Task> tasks, Status status);
}