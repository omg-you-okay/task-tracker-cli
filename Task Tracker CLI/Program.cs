using Task_Tracker_CLI;

var arguments = Environment.GetCommandLineArgs();

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

    if (!File.Exists(TaskTracker.PathToFile))
    {
      Console.WriteLine("Add at leat 1 task");
      return;
    }

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

    if (!File.Exists(TaskTracker.PathToFile))
    {
      Console.WriteLine("Add at leat 1 task");
      return;
    }

    if (arguments.Length < 3)
    {
      Errors.NoId();
      return;
    }

    TaskTracker.DeleteTask(Int32.Parse(arguments[2]));
    break;

  case "mark-in-progress":

    if (!File.Exists(TaskTracker.PathToFile))
    {
      Console.WriteLine("Add at leat 1 task");
      return;
    }

    if (arguments.Length < 3)
    {
      Errors.NoId();
      return;
    }

    TaskTracker.MarkInProgress(int.Parse(arguments[2]));
    break;

  case "mark-done":

    if (!File.Exists(TaskTracker.PathToFile))
    {
      Console.WriteLine("Add at leat 1 task");
      return;
    }

    if (arguments.Length < 3)
    {
      Errors.NoId();
      return;
    }

    TaskTracker.MarkDone(int.Parse(arguments[2]));
    break;

  case "list":

    if (!File.Exists(TaskTracker.PathToFile))
    {
      Console.WriteLine("Add at leat 1 task");
      return;
    }

    TaskTracker.List(arguments);
    break;

  default:
    Console.WriteLine("wrong command");
    break;
}