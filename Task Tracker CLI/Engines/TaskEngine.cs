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
    List<Task> filteredTasks = [];

    switch (status)
    {
      case Status.InProgress:
        filteredTasks = tasks.FindAll(task => task.Status == Status.InProgress);
        break;
      case Status.Done:
        filteredTasks = tasks.FindAll(task => task.Status == Status.Done);
        break;
      case Status.Todo:
        filteredTasks = tasks.FindAll(task => task.Status == Status.Todo);
        break;
      case Status.Unknown:
        Console.WriteLine("incorrect status");
        return [];
    }


    return filteredTasks;
  }
}