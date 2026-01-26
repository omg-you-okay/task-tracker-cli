using Task_Tracker_CLI;
using Task_Tracker_CLI.Engines;
using Task_Tracker_CLI.Resources;
using Task_Tracker_CLI.Models;

var arguments = Environment.GetCommandLineArgs();

if (arguments.Length < 2)
{
  Display.ShowManual();
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
    Display.Error("unknown command");
    break;
}

return;

void AddTask()
{
  if (arguments.Length < 3)
  {
    Display.Error("text is missing");
    return;
  }

  var addResult = taskManager.AddTask(arguments[2]);

  switch (addResult.operationResult)
  {
    case OperationResult.Success:
      Display.TaskCreated(addResult.task!.Id, addResult.task!.Description);
      break;
    case OperationResult.EmptyDescription:
      Display.Error("task description can't be empty");
      break;
  }
}

void UpdateTask()
{
  if (!taskManager.HasTasks())
  {
    Display.NoTasksFound();
    return;
  }

  switch (arguments.Length)
  {
    case < 3:
      Display.Error("id is missing");
      return;
    case < 4:
      Display.Error("text is missing");
      return;
  }

  if (!int.TryParse(arguments[2], out var id))
  {
    Display.Error("id must be a number");
    return;
  }

  var (operationResult, task) = taskManager.UpdateTask(id, arguments[3]);

  switch (operationResult)
  {
    case OperationResult.NotFound:
      Display.Error($"No task with id #{arguments[2]}");
      break;
    case OperationResult.Success:
      Display.TaskUpdated(id.ToString());
      break;
  }
}

void DeleteTask()
{
  if (!taskManager.HasTasks())
  {
    Display.NoTasksFound();
    return;
  }

  if (arguments.Length < 3)
  {
    Display.Error("id is missing");
    return;
  }

  if (!int.TryParse(arguments[2], out var id))
  {
    Display.Error("id must be a number");
    return;
  }

  var deleteResult = taskManager.DeleteTask(id);

  switch (deleteResult.operationResult)
  {
    case OperationResult.NotFound:
      Display.Error($"No task with id #{arguments[2]}");
      break;
    case OperationResult.Success:
      Display.TaskDeleted(id.ToString());
      break;
  }
}

void MarkInProgress()
{
  if (!taskManager.HasTasks())
  {
    Display.NoTasksFound();
    return;
  }

  if (arguments.Length < 3)
  {
    Display.Error("id is missing");
    return;
  }

  if (!int.TryParse(arguments[2], out var id))
  {
    Display.Error("id must be a number");
    return;
  }

  var markInProgressResult = taskManager.MarkInProgress(id);

  switch (markInProgressResult.operationResult)
  {
    case OperationResult.NotFound:
      Display.Error($"No task with id #{arguments[2]}");
      break;
    case OperationResult.AlreadyInProgress:
      Display.Error("already in progress");
      break;
    case OperationResult.Success:
      Display.TaskMarkedInProgress(id.ToString());
      break;
  }
}

void MarkDone()
{
  if (!taskManager.HasTasks())
  {
    Display.NoTasksFound();
    return;
  }

  if (arguments.Length < 3)
  {
    Display.Error("id is missing");
    return;
  }
  
  if (!int.TryParse(arguments[2], out var id))
  {
    Display.Error("id must be a number");
    return;
  }

  var markDoneResult = taskManager.MarkDone(id);
  switch (markDoneResult.operationResult)
  {
    case OperationResult.NotFound:
      Display.Error($"No task with id #{arguments[2]}");
      break;
    case OperationResult.AlreadyDone:
      Display.Error("already done");
      break;
    case OperationResult.Success:
      Display.TaskMarkedDone(id.ToString());
      break;
  }
}

void ListTasks()
{
  if (!taskManager.HasTasks())
  {
    Display.NoTasksFound();
    return;
  }

  Status? status = null;

  if (arguments.Length > 2)
  {
    status = ParseStatus(arguments[2]);
    if (status == null)
    {
      Display.Error($"invalid status: {arguments[2]}");
      return;
    }
  }

  var tasks = taskManager.List(status);

  if (tasks.Count == 0)
  {
    Display.NoTasksFound(arguments[2]);
    return;
  }

  Display.PrintTaskList(tasks);
  
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