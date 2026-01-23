namespace Task_Tracker_CLI.Models;

public enum OperationResult
{
  Success,
  NotFound,
  AlreadyInProgress,
  AlreadyDone,
  InvalidInput,
  EmptyDescription
}
