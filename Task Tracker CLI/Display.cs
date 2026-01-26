namespace Task_Tracker_CLI;

public static class Display
{
  private static void WriteColored(string text, ConsoleColor color)
  {
    Console.ForegroundColor = color;
    Console.Write(text);
    Console.ResetColor();
  }

  private static void WriteLineColored(string text, ConsoleColor color)
  {
    Console.ForegroundColor = color;
    Console.WriteLine(text);
    Console.ResetColor();
  }

  private static void WriteId(string id)
  {
    WriteColored($"#{id}", ConsoleColor.Cyan);
  }

  public static void Error(string message)
  {
    WriteColored("[!] ", ConsoleColor.Red);
    WriteLineColored(message, ConsoleColor.Red);
  }

  public static void TaskCreated(string id, string description)
  {
    WriteColored("[+]", ConsoleColor.Green);
    Console.Write("Task ");
    WriteId(id);
    Console.Write(" created: ");
    Console.WriteLine(description);
  }

  public static void TaskUpdated(string id)
  {
    WriteColored("[~] ", ConsoleColor.Green);
    Console.Write("Task ");
    WriteId(id);
    Console.WriteLine(" updated");
  }

  public static void TaskDeleted(string id)
  {
    WriteColored("[-] ", ConsoleColor.Red);
    Console.Write("Task ");
    WriteId(id);
    Console.WriteLine(" deleted");
  }

  public static void TaskMarkedInProgress(string id)
  {
    WriteColored("[✓] ", ConsoleColor.Yellow);
    Console.Write("Task ");
    WriteId(id);
    Console.WriteLine(" marked in progress");
  }

  public static void TaskMarkedDone(string id)
  {
    WriteColored("[✓] ", ConsoleColor.Green);
    Console.Write("Task ");
    WriteId(id);
    Console.WriteLine(" marked done");
  }

  public static void PrintTask(Task task)
  {
    var (icon, color) = task.Status switch
    {
      Status.Todo => ("○", ConsoleColor.White),
      Status.InProgress => ("◐", ConsoleColor.Yellow),
      Status.Done => ("●", ConsoleColor.Green),
      _ => ("?", ConsoleColor.Gray)
    };

    WriteId(task.Id);
    Console.Write(" ");
    WriteColored(icon, color);
    WriteColored($" {task.Status}", color);
    Console.WriteLine();
    Console.WriteLine($"   {task.Description}");
  }

  public static void PrintTaskList(List<Task> tasks)
  {
    foreach (var task in tasks)
    {
      PrintTask(task);
      Console.WriteLine();
    }
  }
  
  public static void ShowManual()
  {
    Console.WriteLine();
    WriteLineColored("  Task Tracker CLI", ConsoleColor.Cyan);
    Console.WriteLine();
    
    WriteColored("  add", ConsoleColor.Green);
    Console.WriteLine(" \"task description\"");
    
    WriteColored("  update", ConsoleColor.Yellow);
    WriteColored(" <id>", ConsoleColor.Cyan);
    Console.WriteLine(" \"new description\"");
    
    WriteColored("  delete", ConsoleColor.Red);
    WriteColored(" <id>", ConsoleColor.Cyan);
    Console.WriteLine();
    
    WriteColored("  mark-in-progress", ConsoleColor.Yellow);
    WriteColored(" <id>", ConsoleColor.Cyan);
    Console.WriteLine();
    
    WriteColored("  mark-done", ConsoleColor.Green);
    WriteColored(" <id>", ConsoleColor.Cyan);
    Console.WriteLine();
    
    WriteColored("  list", ConsoleColor.White);
    Console.WriteLine(" [todo | in-progress | done]");
    
    Console.WriteLine();
  }

  public static void NoTasksFound(string? filter = null)
  {
    WriteColored("[!] ", ConsoleColor.Yellow);
    Console.WriteLine(filter != null
      ? $"No tasks with status: {filter}"
      : "No tasks yet. Add one with: add \"your task\"");
  }
}