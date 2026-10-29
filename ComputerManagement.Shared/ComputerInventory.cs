namespace ComputerManagement.Shared;

//Specialized collection of computers, accessible with indexers
public class ComputerInventory
{
    public List<Computer> Computers { get; set; } = new();

    public Computer? this[int index]
    {
        get => Computers.FirstOrDefault(computer => computer.Id == index);
    }

    public Computer? this[string reference]
    {
        get => Computers.FirstOrDefault(computer => computer.Name == reference);
    }
}
