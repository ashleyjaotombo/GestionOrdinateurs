using ComputerManagement.API;
using ComputerManagement.Shared;
using Microsoft.EntityFrameworkCore;

namespace ComputerManagement.Tests;

public class RepairServiceTest
{

    private ComputerParkContext CreerContexte()
    {
        var options =
            new DbContextOptionsBuilder<ComputerParkContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        return new ComputerParkContext(options);
    }

    [Fact]
    public void CreateRepair_AddsRepairToContext()
    {
        // Arrange
        //Create context
        ComputerParkContext context = CreerContexte();

        //Add the computer to repair in the context
        Computer computer = new Computer
        {
            Id = 1,
            Name = "Test computer",
            Ram = "8 GB",
            Brand = "Dell",
            Processor = "Intel",
            Stock = 1,
            Price = 500,
            Status = ComputerStatus.Broken,
            Characteristic = new Characteristic(8, 256, ComputerType.Portable, "Intel")
        };
        context.Computers.Add(computer);
        context.SaveChanges();

        //Initialize the service with the context
        RepairService repairService = new RepairService(context, new LogService());

        // Act
        //Create a repair for the computer
        Repair repair = repairService.CreateRepair(computer);

        // Assert
        Assert.Equal(1, repair.ComputerId);
        Assert.Equal("En cours", repair.Status);
        Assert.Single(context.Repairs);
    }
}
