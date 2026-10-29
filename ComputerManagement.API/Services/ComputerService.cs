using ComputerManagement.Shared;
using Microsoft.EntityFrameworkCore;

namespace ComputerManagement.API;

public class ComputerService : IComputerService
{
    private readonly ComputerParkContext _context;

    public ComputerService(ComputerParkContext context)
    {
        _context = context;
    }

    public event EventHandler<ComputerAddedEventArgs>? ComputerAdded;
    public event EventHandler<ComputerUpdatedEventArgs>? ComputerUpdated;
    public event EventHandler<ComputerDeletedEventArgs>? ComputerDeleted;


    public async Task<bool> AddComputerAsync(Computer computer)
    {
        _context.Computers.Add(computer);
        await _context.SaveChangesAsync();
        //Invoke event after adding computer
        ComputerAdded?.Invoke(this, new ComputerAddedEventArgs{Name = computer.Name});

        return true;
    }

    public async Task<bool> DeleteComputerAsync(int id)
    {

        Computer? computerToDelete = await _context.Computers.FindAsync(id);
        if (computerToDelete == null)
        {
            //Computer not found
            return false;
        }

        _context.Computers.Remove(computerToDelete);
        await _context.SaveChangesAsync();
        //Invoke event after deleting computer
        ComputerDeleted?.Invoke(this, new ComputerDeletedEventArgs{Name = computerToDelete.Name});

        return true;
    }


    public async Task<Computer?> UpdateComputerAsync(Computer updatedComputer)
    {
        bool computerIsInDb = await _context.Computers.AnyAsync(c => c.Id == updatedComputer.Id);

        if (!computerIsInDb)
        {
            //If the computer is not found
            return null;
        }
        
        _context.Computers.Update(updatedComputer);
        await _context.SaveChangesAsync();
        //Invoke event after updating computer
        ComputerUpdated?.Invoke(this, new ComputerUpdatedEventArgs{Name = updatedComputer.Name});

        return updatedComputer;
    }


    public async Task<List<Computer>> ListComputersAsync()
    {
        List<Computer> listComputers = await _context.Computers.ToListAsync();
        return listComputers;
    }

    public async Task<Computer?> ReturnComputerByIdAsync(int id)
    {
        Computer? computer = await _context.Computers.FindAsync(id);
        if (computer == null)
        {
            return null;
        }
        return computer;
    }


    public async Task<List<Computer>> ListAvailableComputersAsync()
    {
        List<Computer> listComputers = await _context.Computers
        .Where(computer => computer.Stock > 0)
        .ToListAsync();
        
        return listComputers;
    }

    public async Task<List<Computer>> ListComputersByBrandAsync(String brand)
    {
        List<Computer> listComputers = await _context.Computers
        .Where(computer => computer.Brand.ToLower() == brand.ToLower())
        .ToListAsync();

        return listComputers;
    }

    public async Task<List<Computer>> ListComputersOrderByPriceDescAsync()
    {
        List<Computer> listComputers = await _context.Computers
        .OrderByDescending(computer => computer.Price)
        .ToListAsync();

        return listComputers;
    }

    public string Diagnose(Computer computer)
    {
        return computer switch
        {
            {
                Status: ComputerStatus.OutOfService
            }
                => "Remplacement",

            {
                Status: ComputerStatus.Slow,
                Characteristic:
                {
                    RamGo: < 8
                }
            }
                => "Ajouter RAM",

            _
                => "Aucune action"
        };
    }

    //Return the statistics of the computer park as a tuple
    public (int TotalComputer, int AvailableComputer, decimal StockValue, int RepairNumbers)
    CalculateStatistics()
    {
        int totalComputer = _context.Computers.Count();

        //Available computers with a stock > 0
        int availableComputer = _context.Computers.Count(computer => computer.Stock > 0);

        //Value of the stock
        decimal stockValue = (decimal)_context.Computers.Sum(computer => computer.Price * computer.Stock);

        int repairNumbers = _context.Repairs.Count();

        return (totalComputer, availableComputer, stockValue, repairNumbers);
    }
}