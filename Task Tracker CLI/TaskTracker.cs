using System.Text.Json;

namespace Task_Tracker_CLI;

public static class TaskTracker
{
  public static void AddTask(string description)
  {
    var pathToFile = Path.GetFullPath(Path.Join(Directory.GetCurrentDirectory(), "temp.txt"));

    var task = new Task()
    {
      Id = "1",
      Description = description,
      CreatedAt = DateTime.Now,
      Status = Status.Todo,
    };

    if (!File.Exists(pathToFile))
    {
      List<Task> tasks = [task];
      File.WriteAllText(pathToFile, JsonSerializer.Serialize(tasks));
      return;
    }

    var readFile = File.ReadAllText(pathToFile);
    var readTasks = JsonSerializer.Deserialize<List<Task>>(readFile);
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

    File.WriteAllText(pathToFile, JsonSerializer.Serialize(readTasks));
    
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write($"Id: {newId}; Added: ");
    Console.ResetColor();
    Console.WriteLine(description);
  }

  public static void UpdateTask(int id, string description)
  {
    Console.WriteLine($"updating task: {id} with: {description}");
  }

  public static void DeleteTask(int id)
  {
    Console.WriteLine($"deleting task: {id}");
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

    Console.WriteLine("listing all items");
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
}