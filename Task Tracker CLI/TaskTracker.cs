using System.Text.Json;

namespace Task_Tracker_CLI;

public static class TaskTracker
{
  private static readonly string
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
    Console.WriteLine($"updating task: {id} with: {description}");
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
    Console.WriteLine($"now in-progress: {id}");
  }

  public static void MarkDone(int id)
  {
    Console.WriteLine($"now done: {id}");
  }

  public static void List(string[] arguments)
  {
    if (arguments.Length > 2)
    {
      var status = StatusToEnumMapper(arguments[2]);
      ListByStatus(status);
      return;
    }

    var tasks = GetTasks();
    if (tasks == null) return;

    foreach (var task in tasks)
    {
      Console.Write("ID: ");
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine(task.Id);
      Console.ResetColor();
      Console.Write($"STATUS: ");
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine(task.Status);
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine(task.Description);
      Console.ResetColor();
      Console.WriteLine();
    }
  }

  private static void ListByStatus(Status status)
  {
    Console.WriteLine($"listing: {status}");
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

  private static List<Task>? GetTasks()
  {
    return JsonSerializer.Deserialize<List<Task>>(File.ReadAllText(PathToFile));
  }
}