namespace Task_Tracker_CLI;

public static class Errors
{
  public static void ShowManual()
  {
    Console.WriteLine("🥑  task tracker manual");
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("add");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(" \"any text\"");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.Write("update");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write(" <id> ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("\"any text\"");
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Write("delete");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(" <id>");
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.Write("mark-in-progress");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(" <id>");
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.Write("mark-done");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(" <id>");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("list");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write("list");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write(" done");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write(" | ");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write("todo");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write(" | ");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("in-progress ");
    Console.ResetColor();
  }

  public static void CustomError(string text)
  {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(text);
    Console.ResetColor();
  }

  public static void NoId()
  {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("id is missing");
    Console.ResetColor();
  }

  public static void PrintAddInfo(string id, string text)
  {
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write($"Id: {id}; Added: ");
    Console.ResetColor();
    Console.WriteLine(text);
  }

  public static void PrintTaskInfo(Task task)
  {
    Console.Write("ID: ");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(task?.Id);
    Console.ResetColor();
    Console.Write($"STATUS: ");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(task?.Status);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(task?.Description);
    Console.ResetColor();
  }
}