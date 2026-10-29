namespace ComputerManagement.API;

public interface IStockService
{
    int StockSSD { get; }

    void ConsumeSSD();
}