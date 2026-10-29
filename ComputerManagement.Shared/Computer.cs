using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ComputerManagement.Shared;

public enum ComputerStatus
{
    InService, 
    Broken, 
    InMaintenance,
    OutOfService, 
    Slow, 
    Repairable
}
public class Computer
{
    [Key]
    public int Id{ get; init;}
    public required string Name{ get; set; }
    public required string Ram{ get; set; }

    public required string Brand{ get; set; }
    public required string Processor{ get; set; }

    [DefaultValue(0)]
    public required int Stock{ get; set; }

    [DefaultValue(0)]
    public required float Price{ get; set; }
    public ComputerStatus Status{get; set;}

    [DefaultValue(false)]
    public bool Sold{ get; set; } = false;

    [JsonIgnore]
    public List<Repair> Repairs { get; set; } = new();

    public required Characteristic Characteristic { get; set; }

    public virtual void ViewRecord()
    {
        Console.WriteLine($"Id: {Id}, Name: {Name}, RAM: {Ram}, Processor: {Processor}, Stock : {Stock}, Status: {Status}");
    }

    public virtual void ViewType()
    {
        Console.WriteLine("Computer");
    }

    public virtual void Start()
    {
        Console.WriteLine("Computer started up");
    }
}