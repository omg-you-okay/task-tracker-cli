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
var taskManager = new TaskManager(taskRepository, taskEngine);


switch (command)
{
  case "add":
    AddTask();
    break;

  case "update":
    UpdateTask();
    break;

  case "delete":
    DeleteTask();
    break;

  case "mark-in-progress":
    MarkInProgress();
    break;

  case "mark-done":
    MarkDone();
    break;

  case "list":
    ListTasks();
    break;

  default:
    Console.WriteLine("wrong command");
    break;
}

return;

void AddTask()
{
  if (arguments.Length < 3)
  {
    Errors.CustomError("text is missing");
    return;
  }

  var addResult = taskManager.AddTask(arguments[2]);

  switch (addResult.operationResult)
  {
    case OperationResult.Success:
      Errors.PrintAddInfo(addResult.task!.Id, addResult.task!.Description);
      break;
    case OperationResult.EmptyDescription:
      Console.WriteLine("task description can't be empty");
      break;
  }
}

void UpdateTask()
{
  if (!taskManager.HasTasks())
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

  if (!int.TryParse(arguments[2], out var id))
  {
    Errors.CustomError("id must be a number");
    return;
  }

  var (operationResult, task) = taskManager.UpdateTask(id, arguments[3]);

  switch (operationResult)
  {
    case OperationResult.NotFound:
      Errors.TaskNotFound(arguments[2]);
      break;
    case OperationResult.Success:
      Errors.PrintTaskInfo(task!);
      break;
  }
}

void DeleteTask()
{
  if (!taskManager.HasTasks())
  {
    Console.WriteLine("Add at leat 1 task");
    return;
  }

  if (arguments.Length < 3)
  {
    Errors.NoId();
    return;
  }

  if (!int.TryParse(arguments[2], out var id))
  {
    Errors.CustomError("id must be a number");
    return;
  }

  var deleteResult = taskManager.DeleteTask(id);

  switch (deleteResult.operationResult)
  {
    case OperationResult.NotFound:
      Errors.TaskNotFound(arguments[2]);
      break;
    case OperationResult.Success:
      Errors.CustomError($"you removed task: {deleteResult.task!.Id}");
      break;
  }
}

void MarkInProgress()
{
  if (!taskManager.HasTasks())
  {
    Console.WriteLine("Add at leat 1 task");
    return;
  }

  if (arguments.Length < 3)
  {
    Errors.NoId();
    return;
  }

  if (!int.TryParse(arguments[2], out var id))
  {
    Errors.CustomError("id must be a number");
    return;
  }

  var markInProgressResult = taskManager.MarkInProgress(id);

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
}

void MarkDone()
{
  if (!taskManager.HasTasks())
  {
    Console.WriteLine("Add at leat 1 task");
    return;
  }

  if (arguments.Length < 3)
  {
    Errors.NoId();
    return;
  }
  
  if (!int.TryParse(arguments[2], out var id))
  {
    Errors.CustomError("id must be a number");
    return;
  }

  var markDoneResult = taskManager.MarkDone(id);
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
}

void ListTasks()
{
  if (!taskManager.HasTasks())
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

  var tasks = taskManager.List(status);

  if (tasks.Count == 0)
  {
    Console.WriteLine($"you don't have anything {status}");
    return;
  }

  Errors.PrintList(tasks);
}

Status? ParseStatus(string input)
{
  return input switch
  {
    "todo" => Status.Todo,
    "in-progress" => Status.InProgress,
    "done" => Status.Done,
    _ => null
  };
}