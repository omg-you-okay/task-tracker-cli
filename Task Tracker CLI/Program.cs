using Task_Tracker_CLI;
using Task_Tracker_CLI.Engines;
using Task_Tracker_CLI.Resources;

var arguments = Environment.GetCommandLineArgs();

if (arguments.Length < 2)
{
  Errors.ShowManual();
  return;
}

var command = arguments[1];

var taskRepository = new TaskRepositoryResource(Path.Join(Directory.GetCurrentDirectory(), "tasks.json"));
var taskEngine = new TaskEngine();
var taskTracker = new TaskTracker(taskRepository, taskEngine);


switch (command)
{
  case "add":
    if (arguments.Length < 3)
    {
      Errors.CustomError("text is missing");
      return;
    }

    taskTracker.AddTask(arguments[2]);
    break;

  case "update":

    if (!taskTracker.HasTasks())
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

    taskTracker.UpdateTask(int.Parse(arguments[2]), arguments[3]);
    break;

  case "delete":

    if (!taskTracker.HasTasks())
    {
      Console.WriteLine("Add at leat 1 task");
      return;
    }

    if (arguments.Length < 3)
    {
      Errors.NoId();
      return;
    }

    taskTracker.DeleteTask(Int32.Parse(arguments[2]));
    break;

  case "mark-in-progress":

    if (!taskTracker.HasTasks())
    {
      Console.WriteLine("Add at leat 1 task");
      return;
    }

    if (arguments.Length < 3)
    {
      Errors.NoId();
      return;
    }

    taskTracker.MarkInProgress(int.Parse(arguments[2]));
    break;

  case "mark-done":

    if (!taskTracker.HasTasks())
    {
      Console.WriteLine("Add at leat 1 task");
      return;
    }

    if (arguments.Length < 3)
    {
      Errors.NoId();
      return;
    }

    taskTracker.MarkDone(int.Parse(arguments[2]));
    break;

  case "list":

    if (!taskTracker.HasTasks())
    {
      Console.WriteLine("Add at leat 1 task");
      return;
    }

    var statusFilter = arguments.Length > 2 ? arguments[2] : null;
    taskTracker.List(statusFilter);
    break;

  default:
    Console.WriteLine("wrong command");
    break;
}