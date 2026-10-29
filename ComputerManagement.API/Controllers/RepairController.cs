using ComputerManagement.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace ComputerManagement.API;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RepairController : ControllerBase
{
    private readonly IRepairService _repairService;
    private readonly IComputerService _computerService;

    private readonly IStockService _stockService;

    public RepairController(IRepairService repairService, IComputerService computerService, IStockService stockService)
    {
        _repairService = repairService;
        _computerService = computerService;
        _stockService = stockService;
    }


    [Authorize(Roles = "Admin")]
    [HttpPost()]
    public async Task<IActionResult> SimulateRepair()
    {
        //Recuperate a list of computers
        var listOrdinateurs = await _computerService.ListComputersAsync();
        if (listOrdinateurs.Count < 3)
            return BadRequest("Need at least 3 computers");

        await Task.WhenAll(
            _repairService.SimulateRepairAsync(listOrdinateurs[0]),
            _repairService.SimulateRepairAsync(listOrdinateurs[1]),
            _repairService.SimulateRepairAsync(listOrdinateurs[2])
        );
        return Ok();
    }

    [HttpGet()]
    public async Task<IActionResult> ListRepairs()
    {
        var repairs = await _repairService.ListRepairsAsync();
        return Ok(repairs);
    }


    [HttpGet("GetStockSSD")]
    public ActionResult<int> GetStockSSD()
    {
        Parallel.For(0, 10, i => _stockService.ConsumeSSD());

        return Ok(_stockService.StockSSD);
    }


    [HttpGet("history")]
    public async Task<ActionResult<List<RepairService.RepairHistoryRecord>>> GetRepairHistory()
    {
        var history = await _repairService.GetRepairHistoryAsync();
        return Ok(history);
    }

}