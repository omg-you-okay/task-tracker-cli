namespace Task_Tracker_CLI.Engines;

public class TaskEngine : ITaskEngine
{
  public string GenerateNextId(List<Task> existingTasks)
  {
    List<int> ids = [];

    foreach (var readTask in existingTasks)
    {
      ids.Add(int.Parse(readTask.Id));
    }

    ids.Sort();

    return (ids.LastOrDefault() + 1).ToString();
  }

  public List<Task> FilterByStatus(List<Task> tasks, Status status)
  {
    if (status == Status.Unknown) return [];

    return tasks.FindAll(task => task.Status == status);
  }
}