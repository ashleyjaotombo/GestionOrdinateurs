using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerManagement.Shared;

public class Repair{
    [Key]
    public int repairId { get; set; }

    [ForeignKey(nameof(Computer))]
    public int ComputerId { get; set; }

    public Computer? Computer { get; set; }

    public required string RepairType { get; set; }

    public DateTime StartDate { get; set; }

    public required string Status { get; set;}

    public required string Technician { get; set; }
}
