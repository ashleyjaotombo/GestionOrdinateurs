namespace ComputerManagement.API;

public class LogService : ILogService
{
    //Paths of log files
    public const string RepairsLogPath = "Logs/Repairs/repairs.log";
    public const string ComputersLogPath = "Logs/Computers/computers.log";

    public void Write(string path, string message)
    {
        using var writer =
            new StreamWriter(
                path,
                append: true);

        writer.WriteLine(message);
    }
}
