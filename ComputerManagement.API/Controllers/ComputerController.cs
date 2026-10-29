using Microsoft.AspNetCore.Mvc;
using ComputerManagement.Shared;
using Microsoft.AspNetCore.Authorization;

namespace ComputerManagement.API;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class ComputerController : ControllerBase
{
    private readonly IComputerService _computerService;
    private readonly ILogger<ComputerController> _logger;
    private readonly IConfiguration _configuration;
    private readonly ComputerParkContext _context;
    private readonly ILogService _logService;


    public ComputerController(IComputerService computerService, ILogger<ComputerController> logger, IConfiguration configuration, ComputerParkContext context, ILogService logService)
    {
        _computerService = computerService;
        _logger = logger;
        _configuration = configuration;
        _context = context;
        _logService = logService;
        _computerService.ComputerAdded   += PrintComputerAddedMessage; //Abonnement à l’évènement
        _computerService.ComputerUpdated += PrintComputerUpdatedMessage; //Abonnement à l’évènement
        _computerService.ComputerDeleted += PrintComputerDeletedMessage; //Abonnement à l’évènement
    }

    //Subscriptions to events
    void PrintComputerAddedMessage(object? sender, ComputerAddedEventArgs e)
    {
        Console.WriteLine(
        $"New computer added : {e.Name}");
        //Write the action in the logs with the date of the day
        _logService.Write(LogService.ComputersLogPath, $"{DateTime.Now:dd/MM/yyyy} - New computer added : {e.Name}");
    }

    void PrintComputerUpdatedMessage(object? sender, ComputerUpdatedEventArgs e)
    {
        Console.WriteLine(
        $"Computer updated : {e.Name}");
        //Write the action in the logs with the date of the day
        _logService.Write(LogService.ComputersLogPath, $"{DateTime.Now:dd/MM/yyyy} - Computer updated : {e.Name}");
    }

    void PrintComputerDeletedMessage(object? sender, ComputerDeletedEventArgs e)
    {
        Console.WriteLine(
        $"Computer deleted : {e.Name}");
        //Write the action in the logs with the date of the day
        _logService.Write(LogService.ComputersLogPath, $"{DateTime.Now:dd/MM/yyyy} - Computer deleted : {e.Name}");
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetComputerById(int id)
    {
        Computer? computer = await _computerService.ReturnComputerByIdAsync(id);
        if (computer == null)
        {
            _logger.LogWarning("Computer not found");
            return NotFound("Computer not found");
        }
        _logger.LogInformation("Computer founded");
        return Ok(computer);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllComputers()
    {
        List<Computer> listComputers = await _computerService.ListComputersAsync();
        //Returns 200
        _logger.LogInformation("Return all computers");
        return Ok(listComputers);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddComputer(Computer computer)
    {
        bool result = await _computerService.AddComputerAsync(computer);

        if (!result)
        {
            //Return 400
            _logger.LogWarning("Computer not added");
            return BadRequest("Computer not added");
        }

        //Return 201
        _logger.LogInformation("Computer added");
        return Created($"/api/Computer/{computer.Id}", computer);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComputer(Computer computer)
    {
        Computer? updatedComputer = await _computerService.UpdateComputerAsync(computer);

        if (updatedComputer == null)
        {
            //Return 404
            _logger.LogWarning("Computer not found");
            return NotFound("Computer not found");
        }
        

        //Return 200
        _logger.LogInformation("Computer updated");
        return Ok(computer);
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComputer(int id)
    {
        bool deleted = await _computerService.DeleteComputerAsync(id);

        if (deleted == false)
        {
            //Return 404
            _logger.LogWarning("Computer not found");
            return NotFound("Computer not found");
        }

        //Return 204
        _logger.LogInformation("Computer was deleted");
        return NoContent();
    }

    [HttpGet("info")]
    public IActionResult GetInfo()
    {
        string? appName = _configuration["Application:Name"];
        string? appVersion = _configuration["Application:Version"];

        var info = new
        {
            Name = appName,
            Version = appVersion,
        };

        return Ok(info);
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAllAvailableComputers()
    {
        List<Computer> listComputers = await _computerService.ListAvailableComputersAsync();
        //Returns 200
        _logger.LogInformation("Return all available computers");
        return Ok(listComputers);
    }

    [HttpGet("brand/{name}")]
    public async Task<IActionResult> GetComputersByBrand(String name)
    {
        List<Computer> listComputers = await _computerService.ListComputersByBrandAsync(name);
        //Returns 200
        _logger.LogInformation("Return computers by brand");
        return Ok(listComputers);
    }

    [HttpGet("price")]
    public async Task<IActionResult> GetComputersSortByPrice()
    {
        List<Computer> listComputers = await _computerService.ListComputersOrderByPriceDescAsync();
        //Returns 200
        _logger.LogInformation("Return all available computers");
        return Ok(listComputers);
    }

    [HttpGet("{id}/diagnose")]
    public async Task<IActionResult> GetComputerDiagnosis(int id)
    {
        Computer? computer = await _computerService.ReturnComputerByIdAsync(id);

        if (computer == null)
        {
            return NotFound("Computer not found");
        }

        string diagnosis = _computerService.Diagnose(computer);
        return Ok(diagnosis);
    }

    [HttpGet("statistics")]
    public IActionResult GetStatistics()
    {
        var (total, inStock, stockValue, repairNumbers)
            = _computerService.CalculateStatistics();

        _logger.LogInformation("Return the statistics of computers");
        return Ok(new
        {
            TotalComputer = total,
            AvailableComputer = inStock,
            StockValue = stockValue,
            RepairNumbers = repairNumbers
        });
    }

    [HttpGet("inventory/{name}")]
    public IActionResult Search(string name)
    {
        var inventory = new ComputerInventory();
        inventory.Computers = _context.Computers.ToList();

        var computer = inventory[name];

        if (computer == null)
        {
            return NotFound("Computer not found");
        }

        return Ok(computer);
    }
}