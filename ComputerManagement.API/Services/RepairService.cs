using ComputerManagement.Shared;
using Microsoft.EntityFrameworkCore;

namespace ComputerManagement.API;

public class RepairService : IRepairService
{
    public record RepairHistoryRecord(
        int ComputerId,
        string ComputerName,
        string Technician,
        string RepairType,
        DateTime InterventionDate
    );

    private readonly ComputerParkContext _context;
    private readonly ILogService _logService;

    private readonly object _lock = new object();

    public RepairService(ComputerParkContext context, ILogService logService)
    {
        _context = context;
        _logService = logService;
    }

    //Creation of a repair for a computer
    public Repair CreateRepair(Computer computer)
    {
        //Lock to ensure the concurrency of threads during updating context
        lock (_lock)
        {
            //Initialize a new repair for the computer
            var repair = new Repair
            {
                ComputerId = computer.Id,
                RepairType = "Test",
                StartDate = DateTime.Now,
                Status = "En cours",
                Technician = "John"
            };

            //Add the repair to the database
            _context.Repairs.Add(repair);
            _context.SaveChanges();
            return repair;
        }
    }

    public async Task SimulateRepairAsync(Computer computer)
    {
        var repair = CreateRepair(computer);  
        await Task.Delay(2000);                           

        //Lock to ensure the concurrency of threads during updating context
        lock (_lock)
        {
            repair.Status = "Terminée";
            _context.SaveChanges();                        
        }

        Console.WriteLine(
            $"{computer.Name} réparé");

        //Write the repair in the logs with the date
        _logService.Write(LogService.RepairsLogPath, $"{DateTime.Now:dd/MM/yyyy} - {computer.Name} réparé");
    }



    public async Task<List<Repair>> ListRepairsAsync()
    {
        //Recuperation of the list of repaires who have an idComputer
        var repairs = await _context.Repairs
            .Include(r => r.Computer)
            .ToListAsync();


        return repairs;
    }

    //Return the repair history
    public async Task<List<RepairHistoryRecord>> GetRepairHistoryAsync()
    {
        return await _context.Repairs
            .Select(r => new RepairHistoryRecord(
                r.ComputerId,
                r.Computer!.Name,
                r.Technician,
                r.RepairType,
                r.StartDate))
            .ToListAsync();
    }

}