using Task_Tracker_CLI;

string[] arguments = Environment.GetCommandLineArgs();

if (arguments.Length < 2)
{
  Errors.ShowManual();
  return;
}

var command = arguments[1];

switch (command)
{
  case "add":
    if (arguments.Length < 3)
    {
      Errors.CustomError("text is missing");
      return;
    }
    TaskTracker.AddTask(arguments[2]);
    break;

  case "update":
    if (arguments.Length < 3)
    {
      Errors.NoId();
      return;
    }
    if (arguments.Length < 4)
    {
      Errors.CustomError("text is missing");
      return;
    }
    TaskTracker.UpdateTask(Int32.Parse(arguments[2]), arguments[3]);
    break;

  case "delete":
    if (arguments.Length < 3)
    {
      Errors.NoId();
      return;
    }
    TaskTracker.DeleteTask(Int32.Parse(arguments[2]));
    break;

  case "mark-in-progress":
    if (arguments.Length < 3)
    {
      Errors.NoId();
      return;
    }
    TaskTracker.MarkInProgress(Int32.Parse(arguments[2]));
    break;

  case "mark-done":
    if (arguments.Length < 3)
    {
      Errors.NoId();
      return;
    }
    TaskTracker.MarkDone(Int32.Parse(arguments[2]));
    break;

  case "list":
    TaskTracker.List(arguments);
    break;

  default:
    Console.WriteLine("wrong command");
    break;
}