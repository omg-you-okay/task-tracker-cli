using System.Text.Json;

namespace Task_Tracker_CLI.Resources;

public class TaskRepositoryResource : ITaskRepositoryResource
{
  private readonly string _filePath;

  public TaskRepositoryResource(string filePath)
  {
    _filePath = filePath;
  }

  public bool StorageExists()
  {
    return File.Exists(this._filePath);
  }

  public List<Task> GetAllTasks()
  {
    var json = File.ReadAllText(_filePath);
    return JsonSerializer.Deserialize<List<Task>>(json) ?? [];
  }

  public void SaveAll(List<Task> tasks)
  {
    var options = new JsonSerializerOptions { WriteIndented = true };
    var json = JsonSerializer.Serialize(tasks, options);
    File.WriteAllText(_filePath, json);
  }
}