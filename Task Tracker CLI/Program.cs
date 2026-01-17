using Task_Tracker_CLI;

var arguments = Environment.GetCommandLineArgs();

if (arguments.Length < 2)
{
  Errors.ShowManual();
  return;
}

var command = arguments[1];

// TODO: Handle errors when no file exists
// TODO: identify more errors and edge cases 
// TODO: update;
// TODO: mark-in-progress;
// TODO: mark-done
// TODO: list done | todo | in-progress 

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
    switch (arguments.Length)
    {
      case < 3:
        Errors.NoId();
        return;
      case < 4:
        Errors.CustomError("text is missing");
        return;
    }

    TaskTracker.UpdateTask(int.Parse(arguments[2]), arguments[3]);
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

    TaskTracker.MarkInProgress(int.Parse(arguments[2]));
    break;

  case "mark-done":
    if (arguments.Length < 3)
    {
      Errors.NoId();
      return;
    }

    TaskTracker.MarkDone(int.Parse(arguments[2]));
    break;

  case "list":
    TaskTracker.List(arguments);
    break;

  default:
    Console.WriteLine("wrong command");
    break;
}