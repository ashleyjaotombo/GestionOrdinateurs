namespace ComputerManagement.API;

public class ComputerDeletedEventArgs : EventArgs
{
    public required string Name { get; set; }
}