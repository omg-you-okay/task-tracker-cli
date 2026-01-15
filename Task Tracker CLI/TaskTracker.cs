namespace Task_Tracker_CLI;

public static class TaskTracker
{
  public static void AddTask(string description)
  {
    Console.WriteLine($"adding {description}");
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
        return Status.Unknown;
    }
  }
}