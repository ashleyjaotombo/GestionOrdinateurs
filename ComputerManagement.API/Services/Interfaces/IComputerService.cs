using ComputerManagement.Shared;

namespace ComputerManagement.API;

public interface IComputerService
{
    Task<bool> AddComputerAsync(Computer computer);
    Task<bool> DeleteComputerAsync(int id);
    Task<Computer?> UpdateComputerAsync(Computer ComputerUpdated);
    Task<List<Computer>> ListComputersAsync();
    Task<Computer?> ReturnComputerByIdAsync(int id);

    Task<List<Computer>> ListAvailableComputersAsync();

    Task<List<Computer>> ListComputersByBrandAsync(String brand);

    Task<List<Computer>> ListComputersOrderByPriceDescAsync();

    string Diagnose(Computer computer);

    (int TotalComputer, int AvailableComputer, decimal StockValue, int RepairNumbers) CalculateStatistics();

    event EventHandler<ComputerAddedEventArgs>? ComputerAdded;
    event EventHandler<ComputerUpdatedEventArgs>? ComputerUpdated;
    event EventHandler<ComputerDeletedEventArgs>? ComputerDeleted;
}