namespace Task_Tracker_CLI.Resources;

public interface ITaskRepositoryResource
{
  bool StorageExists();
  List<Task> GetAllTasks();
  void SaveAll(List<Task> tasks);
}