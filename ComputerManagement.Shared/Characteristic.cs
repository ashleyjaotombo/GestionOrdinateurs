namespace ComputerManagement.Shared;

public class Characteristic
{
    public int RamGo { get; set; }
    public ComputerType ComputerType { get; set; }
    public int StorageGo { get; set; }
    public string Processor { get; set; }

    public Characteristic(int ramGo, int storageGo, ComputerType computerType, string processor)
    {
        RamGo = ramGo;
        StorageGo = storageGo;
        ComputerType = computerType;
        Processor = processor;
    }
}
