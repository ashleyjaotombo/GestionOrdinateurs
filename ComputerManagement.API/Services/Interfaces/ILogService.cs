namespace ComputerManagement.API;

public interface ILogService
{
    void Write(string path, string message);
}
