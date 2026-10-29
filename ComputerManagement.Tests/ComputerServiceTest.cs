using ComputerManagement.API;
using ComputerManagement.Shared;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace ComputerManagement.Tests;

public class ComputerServiceTest
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
    public async Task AddComputer_AddsComputerToContext()
    {
        // Arrange
        //Create context
        ComputerParkContext context = CreerContexte();
        //Initialize the service with the context
        ComputerService computerService = new ComputerService(context);

        //Initialize the computer to add
        Computer computer = new Computer
        {
            Id = 10,
            Name = "Test computer",
            Ram = "16 GB",
            Brand = "Huawei", 
            Processor = "Intel",
            Stock = 1,
            Price = 700,
            Status = ComputerStatus.InService,
            Characteristic = new Characteristic(16, 512, ComputerType.Portable, "Intel")
        };

        // Act
        //Add the computer to the context
        bool result = await computerService.AddComputerAsync(computer);

        // Assert
        Assert.True(result);
        Assert.NotNull(await computerService.ReturnComputerByIdAsync(10)); 
    }

    [Fact]
    public async Task DeleteComputer_RemovesComputerFromContext()
    {
        // Arrange
        //Create context
        ComputerParkContext context = CreerContexte();

        // Create a list with two computers
        List<Computer> listComputers = new List<Computer>
        {
            new Computer { Id = 1, Name = "Computer 1", Ram = "8 GB", Brand = "Huawei", Processor = "Intel", Stock = 1, Price = 700, Status = ComputerStatus.InService, Characteristic = new Characteristic(8, 256, ComputerType.Fixe, "Intel") },
            new Computer { Id = 2, Name = "Computer 2", Ram = "16 GB", Brand = "Huawei", Processor = "Ryzen", Stock = 2, Price = 750, Status = ComputerStatus.Broken, Characteristic = new Characteristic(16, 512, ComputerType.Server, "Ryzen") }
        };

        //Adding the 2 computers into the context
        context.Computers.Add(listComputers[0]);
        context.Computers.Add(listComputers[1]);
        context.SaveChanges();

        //Initialize the service with the context
        ComputerService computerService = new ComputerService(context);

        // Act
        bool result = await computerService.DeleteComputerAsync(1);

        // Assert
        Assert.True(result); 
        Assert.Null(await computerService.ReturnComputerByIdAsync(1));
    }

    [Fact]
    public async Task DeleteComputer_ReturnsFalseIfComputerNotFound()
    {
        // Arrange
        //Create context
        ComputerParkContext context = CreerContexte();

        //Create a list of two computers
        List<Computer> listComputers = new List<Computer>
        {
            new Computer { Id = 1, Name = "Computer 1", Ram = "8 GB", Brand = "Huawei", Processor = "Intel", Stock = 1, Price = 700, Status = ComputerStatus.InService, Characteristic = new Characteristic(8, 256, ComputerType.Fixe, "Intel") },
            new Computer { Id = 2, Name = "Computer 2", Ram = "16 GB", Brand = "Huawei", Processor = "Ryzen", Stock = 2, Price = 750, Status = ComputerStatus.Broken, Characteristic = new Characteristic(16, 512, ComputerType.Server, "Ryzen") }
        };

        //Adding the 2 computers into the context
        context.Computers.Add(listComputers[0]);
        context.Computers.Add(listComputers[1]);
        context.SaveChanges();

        //Initialize the service with the context
        ComputerService computerService = new ComputerService(context);

        // Act
        bool result = await computerService.DeleteComputerAsync(3);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateComputer_UpdatesComputerInContext()
    {
        // Arrange
        //Create context
        ComputerParkContext context = CreerContexte();

        //Create a list of two computers
        List<Computer> listComputers = new List<Computer>
        {
            new Computer { Id = 1, Name = "Computer 1", Ram = "8 GB", Brand = "Huawei", Processor = "Intel", Stock = 2, Price = 700, Status = ComputerStatus.InService, Characteristic = new Characteristic(8, 256, ComputerType.Fixe, "Intel") },
            new Computer { Id = 2, Name = "Computer 2", Ram = "16 GB", Brand = "Huawei", Processor = "Ryzen", Stock = 3, Price = 750, Status = ComputerStatus.Broken, Characteristic = new Characteristic(16, 512, ComputerType.Server, "Ryzen") }
        };

        //Adding the 2 computers into the context
        context.Computers.Add(listComputers[0]);
        context.Computers.Add(listComputers[1]);
        context.SaveChanges();

        //Initialize the service with the context
        ComputerService computerService = new ComputerService(context);

        //Modify the computer to update
        listComputers[0].Name = "Computer 1 - New Version";
        listComputers[0].Status = ComputerStatus.Broken;

        // Act
        Computer? result = await computerService.UpdateComputerAsync(listComputers[0]);
        Computer? computer = await computerService.ReturnComputerByIdAsync(1);
        
        // Assert
        computer.Should().NotBeNull();
        computer!.Name.Should().Be("Computer 1 - New Version");
        computer!.Status.Should().Be(ComputerStatus.Broken);
    }

   

    [Fact]
    public async Task ReturnComputerById_ReturnsComputer()
    {
        // Arrange
        //Create context
        ComputerParkContext context = CreerContexte();

        //Create a list of two computers
        List<Computer> listComputers = new List<Computer>
        {
            new Computer { Id = 1, Name = "Computer 1", Ram = "8 GB", Brand = "Huawei", Processor = "Intel", Stock = 1, Price = 700, Status = ComputerStatus.InService, Characteristic = new Characteristic(8, 256, ComputerType.Fixe, "Intel") },
            new Computer { Id = 2, Name = "Computer 2", Ram = "16 GB", Brand = "Huawei", Processor = "Ryzen", Stock = 2, Price = 750, Status = ComputerStatus.Broken, Characteristic = new Characteristic(16, 512, ComputerType.Server, "Ryzen") }
        };

        //Adding the 2 computers into the context
        context.Computers.Add(listComputers[0]);
        context.Computers.Add(listComputers[1]);
        context.SaveChanges();

        //Initialize the service with the context
        ComputerService computerService = new ComputerService(context);

        // Act
        Computer? result = await computerService.ReturnComputerByIdAsync(2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Computer 2", result.Name);
    }

    [Fact]
    public async Task ReturnComputerById_ReturnsNullIfComputerNotFound()
    {
        // Arrange
        //Create context
        ComputerParkContext context = CreerContexte();

        //Create a list of two computers
        List<Computer> listComputers = new List<Computer>
        {
            new Computer { Id = 1, Name = "Computer 1", Ram = "8 GB", Brand = "Huawei", Processor = "Intel", Stock = 1, Price = 700, Status = ComputerStatus.InService, Characteristic = new Characteristic(8, 256, ComputerType.Fixe, "Intel") },
            new Computer { Id = 2, Name = "Computer 2", Ram = "16 GB", Brand = "Huawei", Processor = "Ryzen", Stock = 2, Price = 750, Status = ComputerStatus.Broken, Characteristic = new Characteristic(16, 512, ComputerType.Server, "Ryzen") }
        };

        //Adding the 2 computers into the context
        context.Computers.Add(listComputers[0]);
        context.Computers.Add(listComputers[1]);
        context.SaveChanges();

        //Initialize the service with the context
        ComputerService computerService = new ComputerService(context);

        // Act
        Computer? result = await computerService.ReturnComputerByIdAsync(3);

        // Assert
        Assert.Null(result);
    }
}