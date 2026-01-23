using Task_Tracker_CLI;
using Task_Tracker_CLI.Engines;
using Task_Tracker_CLI.Resources;
using Task_Tracker_CLI.Models;

var arguments = Environment.GetCommandLineArgs();

if (arguments.Length < 2)
{
  Errors.ShowManual();
  return;
}

var command = arguments[1];

var taskRepository = new TaskRepositoryResource(Path.Join(Directory.GetCurrentDirectory(), "tasks.json"));
var taskEngine = new TaskEngine();
var taskTracker = new TaskManager(taskRepository, taskEngine);


switch (command)
{
  case "add":
    if (arguments.Length < 3)
    {
      Errors.CustomError("text is missing");
      return;
    }

    var addResult = taskTracker.AddTask(arguments[2]);

    switch (addResult.operationResult)
    {
      case OperationResult.Success:
        Errors.PrintAddInfo(addResult.task!.Id, addResult.task!.Description);
        break;
      case OperationResult.EmptyDescription:
        Console.WriteLine("task description can't be empty");
        break;
    }

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

    var (operationResult, task) = taskTracker.UpdateTask(int.Parse(arguments[2]), arguments[3]);

    switch (operationResult)
    {
      case OperationResult.NotFound:
        Errors.TaskNotFound(arguments[2]);
        break;
      case OperationResult.Success:
        Errors.PrintTaskInfo(task!);
        break;
    }

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

    var deleteResult = taskTracker.DeleteTask(Int32.Parse(arguments[2]));

    switch (deleteResult.operationResult)
    {
      case OperationResult.NotFound:
        Errors.TaskNotFound(arguments[2]);
        break;
      case OperationResult.Success:
        Errors.CustomError($"you removed task: {deleteResult.task!.Id}");
        break;
    }

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

    var markInProgressResult = taskTracker.MarkInProgress(int.Parse(arguments[2]));

    switch (markInProgressResult.operationResult)
    {
      case OperationResult.NotFound:
        Errors.TaskNotFound(arguments[2]);
        break;
      case OperationResult.AlreadyInProgress:
        Errors.CustomError("already in progress");
        break;
      case OperationResult.Success:
        Errors.PrintTaskInfo(markInProgressResult.task!);
        break;
    }

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

    var markDoneResult = taskTracker.MarkDone(int.Parse(arguments[2]));
    switch (markDoneResult.operationResult)
    {
      case OperationResult.NotFound:
        Errors.TaskNotFound(arguments[2]);
        break;
      case OperationResult.AlreadyDone:
        Errors.CustomError("already done");
        break;
      case OperationResult.Success:
        Errors.PrintTaskInfo(markDoneResult.task!);
        break;
    }

    break;

  case "list":

    if (!taskTracker.HasTasks())
    {
      Console.WriteLine("Add at leat 1 task");
      return;
    }

    Status? status = null;

    if (arguments.Length > 2)
    {
      status = ParseStatus(arguments[2]);
      if (status == null)
      {
        Console.WriteLine($"invalid {arguments[2]}");
        return;
      }
    }

    var tasks = taskTracker.List(status);

    if (tasks.Count == 0)
    {
      Console.WriteLine($"you don't have anything {status}");
      return;
    }

    Errors.PrintList(tasks);
    break;

  default:
    Console.WriteLine("wrong command");
    break;
}

return;

static Status? ParseStatus(string input)
{
  return input switch
  {
    "todo" => Status.Done,
    "in-progress" => Status.InProgress,
    "done" => Status.Done,
    _ => null
  };
}