namespace ComputerManagement.API;

public class ComputerUpdatedEventArgs : EventArgs
{
    public required string Name { get; set; }
}