namespace ComputerManagement.API;

public class ComputerAddedEventArgs : EventArgs
{
    public required string Name { get; set; }
}