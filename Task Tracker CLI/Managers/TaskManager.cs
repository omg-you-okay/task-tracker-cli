using Task_Tracker_CLI.Engines;
using Task_Tracker_CLI.Resources;
using Task_Tracker_CLI.Models;

namespace Task_Tracker_CLI;

public class TaskManager(
  ITaskRepositoryResource taskRepositoryResource,
  ITaskEngine taskEngine
)
{
  public bool HasTasks() => taskRepositoryResource.StorageExists();

  public (OperationResult operationResult, Task? task) AddTask(string description)
  {
    if (description.IsWhiteSpace()) return (OperationResult.EmptyDescription, null);

    var task = new Task()
    {
      Id = "1",
      Description = description,
      CreatedAt = DateTime.Now,
      Status = Status.Todo,
    };

    if (!taskRepositoryResource.StorageExists())
    {
      List<Task> tasks = [task];
      taskRepositoryResource.SaveAll(tasks);

      return (OperationResult.Success, task);
    }

    var readTasks = taskRepositoryResource.GetAllTasks();
    var newId = taskEngine.GenerateNextId(readTasks);

    task.Id = newId;
    readTasks.Add(task);

    taskRepositoryResource.SaveAll(readTasks);

    return (OperationResult.Success, task);
  }

  public (OperationResult result, Task? task) UpdateTask(int id, string description)
  {
    var tasks = taskRepositoryResource.GetAllTasks();
    var taskToUpdate = tasks.FirstOrDefault(task => task.Id == id.ToString());

    if (taskToUpdate == null) return (OperationResult.NotFound, null);

    taskToUpdate.Description = description;
    taskToUpdate.UpdatedAt = DateTime.Now;
    taskRepositoryResource.SaveAll(tasks);

    return (OperationResult.Success, taskToUpdate);
  }

  public (OperationResult operationResult, Task? task) DeleteTask(int id)
  {
    var tasks = taskRepositoryResource.GetAllTasks();
    var taskToDelete = tasks.Find(task => task.Id == id.ToString());

    if (taskToDelete == null) return (OperationResult.NotFound, null);

    tasks.Remove(taskToDelete);
    taskRepositoryResource.SaveAll(tasks);

    return (OperationResult.Success, taskToDelete);
  }

  public (OperationResult operationResult, Task? task) MarkInProgress(int id)
  {
    var tasks = taskRepositoryResource.GetAllTasks();
    var taskToUpdate = tasks.FirstOrDefault(task => task.Id == id.ToString());

    if (taskToUpdate == null) return (OperationResult.NotFound, null);
    if (taskToUpdate.Status == Status.InProgress) return (OperationResult.AlreadyInProgress, null);

    taskToUpdate.Status = Status.InProgress;
    taskToUpdate.UpdatedAt = DateTime.Now;

    taskRepositoryResource.SaveAll(tasks);

    return (OperationResult.Success, taskToUpdate);
  }

  public (OperationResult operationResult, Task? task) MarkDone(int id)
  {
    var tasks = taskRepositoryResource.GetAllTasks();
    var taskToUpdate = tasks.FirstOrDefault(task => task.Id == id.ToString());

    if (taskToUpdate == null) return (OperationResult.NotFound, null);
    if (taskToUpdate.Status == Status.Done) return (OperationResult.AlreadyDone, null);

    taskToUpdate.Status = Status.Done;
    taskToUpdate.UpdatedAt = DateTime.Now;

    taskRepositoryResource.SaveAll(tasks);

    return (OperationResult.Success, taskToUpdate);
  }

  public List<Task> List(Status? status)
  {
    var tasks = taskRepositoryResource.GetAllTasks();
    
    if (status.HasValue)
    {
      var filteredTasks = taskEngine.FilterByStatus(tasks, status.Value);
      return filteredTasks;
    }

    return tasks;
  }
}