using Task_Tracker_CLI;

string[] arguments = Environment.GetCommandLineArgs();

var command = arguments[1];

switch (command)
{
  case "add":
    TaskTracker.AddTask(arguments[2]);
    break;

  case "update":
    TaskTracker.UpdateTask(Int32.Parse(arguments[2]), arguments[3]);
    break;

  case "delete":
    TaskTracker.DeleteTask(Int32.Parse(arguments[2]));
    break;

  case "mark-in-progress":
    TaskTracker.MarkInProgress(Int32.Parse(arguments[2]));
    break;

  case "mark-done":
    TaskTracker.MarkDone(Int32.Parse(arguments[2]));
    break;

  case "list":
    TaskTracker.List(arguments);
    break;

  default:
    Console.WriteLine("wrong command");
    break;
}