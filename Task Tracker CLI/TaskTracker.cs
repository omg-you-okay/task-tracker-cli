using System.Text.Json;

namespace Task_Tracker_CLI;

public static class TaskTracker
{
  public static readonly string
    PathToFile = Path.Join(Directory.GetCurrentDirectory(), "tasks.json");

  public static void AddTask(string description)
  {
    var task = new Task()
    {
      Id = "1",
      Description = description,
      CreatedAt = DateTime.Now,
      Status = Status.Todo,
    };

    if (!File.Exists(PathToFile))
    {
      List<Task> tasks = [task];
      UpdateFile(tasks);

      Errors.PrintAddInfo(task.Id, description);
      return;
    }


    var readTasks = GetTasks();
    if (readTasks == null) return;

    List<int> ids = [];

    foreach (var readTask in readTasks)
    {
      ids.Add(int.Parse(readTask.Id));
    }

    ids.Sort();
    var newId = (ids.LastOrDefault() + 1).ToString();
    task.Id = newId;
    readTasks.Add(task);

    UpdateFile(readTasks);

    Errors.PrintAddInfo(newId, description);
  }

  public static void UpdateTask(int id, string description)
  {
    var tasks = GetTasks();
    if (tasks == null) return;

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
    UpdateFile(tasks);

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("updated");
    Console.ResetColor();
    Errors.PrintTaskInfo(taskToUpdate);
  }

  public static void DeleteTask(int id)
  {
    var tasks = GetTasks();
    var taskToDelete = tasks?.Find(task => task.Id == id.ToString());
    if (taskToDelete == null)
    {
      Console.Write($"there is no task with id ");
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine(id);
      return;
    }

    tasks?.Remove(taskToDelete);

    if (tasks != null) UpdateFile(tasks);

    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("you removed task: ");
    Console.WriteLine(taskToDelete.Id);
  }

  public static void MarkInProgress(int id)
  {
    var tasks = GetTasks();
    if (tasks == null) return;

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

    UpdateFile(tasks);
    Errors.PrintTaskInfo(taskToUpdate);
  }

  public static void MarkDone(int id)
  {
    var tasks = GetTasks();
    if (tasks == null) return;

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

    UpdateFile(tasks);
    Errors.PrintTaskInfo(taskToUpdate);
  }

  public static void List(string[] arguments)
  {
    var tasks = GetTasks();
    if (tasks == null) return;

    if (arguments.Length > 2)
    {
      var status = StatusToEnumMapper(arguments[2]);
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

  private static void UpdateFile(List<Task> tasks)
  {
    var options = new JsonSerializerOptions { WriteIndented = true };
    File.WriteAllText(PathToFile, JsonSerializer.Serialize(tasks, options));
  }

  private static List<Task>? GetTasks() => JsonSerializer.Deserialize<List<Task>>(File.ReadAllText(PathToFile));
}