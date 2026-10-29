
using ComputerManagement.Shared;

namespace ComputerManagement.API;

public interface IRepairService
{
    Task SimulateRepairAsync(Computer computer);

    Task<List<Repair>> ListRepairsAsync();

    Repair CreateRepair(Computer computer);

    Task<List<RepairService.RepairHistoryRecord>> GetRepairHistoryAsync();
}