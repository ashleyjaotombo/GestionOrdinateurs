namespace ComputerManagement.API;

public class StockService : IStockService
{
    private readonly object _lock = new object();

    public int StockSSD { get; private set; } = 10;

    public void ConsumeSSD()
    {
        //Lock to ensure the concurrency of threads during updating context
        lock (_lock)
        {
            if (StockSSD <= 0)
            {
                Console.WriteLine("Stock is already empty");
                return;
            }
            Task.Delay(10).Wait();
            StockSSD--;
            Console.WriteLine("Stock decreased by 1, actual stock: " + StockSSD);
        }
    }
}