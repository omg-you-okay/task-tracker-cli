using Task_Tracker_CLI.Resources;

namespace Task_Tracker_CLI;

public class TaskTracker
{
  private ITaskRepositoryResource _taskRepositoryResource;

  public TaskTracker(ITaskRepositoryResource taskRepositoryResource)
  {
    this._taskRepositoryResource = taskRepositoryResource;
  }

  public bool HasTasks() => _taskRepositoryResource.StorageExists();

  public void AddTask(string description)
  {
    if (description.IsWhiteSpace())
    {
      Console.WriteLine("task description can't be empty");
      return;
    }

    var task = new Task()
    {
      Id = "1",
      Description = description,
      CreatedAt = DateTime.Now,
      Status = Status.Todo,
    };

    if (!_taskRepositoryResource.StorageExists())
    {
      List<Task> tasks = [task];
      _taskRepositoryResource.SaveAll(tasks);

      Errors.PrintAddInfo(task.Id, description);
      return;
    }


    var readTasks = _taskRepositoryResource.GetAllTasks();

    List<int> ids = [];

    foreach (var readTask in readTasks)
    {
      ids.Add(int.Parse(readTask.Id));
    }

    ids.Sort();
    var newId = (ids.LastOrDefault() + 1).ToString();
    task.Id = newId;
    readTasks.Add(task);

    _taskRepositoryResource.SaveAll(readTasks);

    Errors.PrintAddInfo(newId, description);
  }

  public void UpdateTask(int id, string description)
  {
    var tasks = _taskRepositoryResource.GetAllTasks();

    var taskToUpdate = tasks.FirstOrDefault(task => task.Id == id.ToString());
    if (taskToUpdate == null)
    {
      Console.Write($"there is no task with id ");
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine(id);
      return;
    }

    taskToUpdate.Description = description;
    taskToUpdate.UpdatedAt = DateTime.Now;
    _taskRepositoryResource.SaveAll(tasks);

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("updated");
    Console.ResetColor();
    Errors.PrintTaskInfo(taskToUpdate);
  }

  public void DeleteTask(int id)
  {
    var tasks = _taskRepositoryResource.GetAllTasks();
    var taskToDelete = tasks?.Find(task => task.Id == id.ToString());
    if (taskToDelete == null)
    {
      Console.Write($"there is no task with id ");
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine(id);
      return;
    }

    tasks?.Remove(taskToDelete);

    if (tasks != null) _taskRepositoryResource.SaveAll(tasks);

    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("you removed task: ");
    Console.WriteLine(taskToDelete.Id);
  }

  public void MarkInProgress(int id)
  {
    var tasks = _taskRepositoryResource.GetAllTasks();

    var taskToUpdate = tasks.FirstOrDefault(task => task.Id == id.ToString());

    if (taskToUpdate?.Status == Status.InProgress)
    {
      Console.WriteLine("already in progress");
      return;
    }

    if (taskToUpdate == null)
    {
      Console.Write($"there is no task with id ");
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine(id);
      return;
    }

    taskToUpdate.Status = Status.InProgress;
    taskToUpdate.UpdatedAt = DateTime.Now;

    _taskRepositoryResource.SaveAll(tasks);
    Errors.PrintTaskInfo(taskToUpdate);
  }

  public void MarkDone(int id)
  {
    var tasks = _taskRepositoryResource.GetAllTasks();

    var taskToUpdate = tasks.FirstOrDefault(task => task.Id == id.ToString());
    if (taskToUpdate?.Status == Status.Done)
    {
      Console.Write("already in done, ");
      Console.WriteLine("congrats");
      Console.ResetColor();
      return;
    }

    if (taskToUpdate == null)
    {
      Console.Write($"there is no task with id ");
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine(id);
      return;
    }

    taskToUpdate.Status = Status.Done;
    taskToUpdate.UpdatedAt = DateTime.Now;

    _taskRepositoryResource.SaveAll(tasks);
    Errors.PrintTaskInfo(taskToUpdate);
  }

  public void List(string? statusFilter)
  {
    var tasks = _taskRepositoryResource.GetAllTasks();

    if (statusFilter != null)
    {
      var status = StatusToEnumMapper(statusFilter);
      ListByStatus(status, tasks);
      return;
    }

    foreach (var task in tasks)
    {
      Errors.PrintTaskInfo(task);
      Console.WriteLine();
    }
  }

  private static void ListByStatus(Status status, List<Task> tasks)
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
        return;
    }

    if (filteredTasks.Count == 0)
    {
      Console.WriteLine($"you don't have anything {status}");
      return;
    }

    foreach (var task in filteredTasks)
    {
      Errors.PrintTaskInfo(task);
      Console.WriteLine();
    }
  }

  private static Status StatusToEnumMapper(string status)
  {
    switch (status)
    {
      case "todo":
        return Status.Todo;
      case "in-progress":
        return Status.InProgress;
      case "done":
        return Status.Done;
      default:
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"no such category: {status}");
        Console.ResetColor();
        return Status.Unknown;
    }
  }
}