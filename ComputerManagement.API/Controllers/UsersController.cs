using ComputerManagement.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ComputerManagement.API;

[Route("api/[controller]")]
[ApiController]
public class UsersController(ComputerParkContext context) : ControllerBase
{
    private readonly ComputerParkContext _context = context;
    
    [HttpGet("userbylogin/{login}")]
    public ActionResult<User> GetUserByLogin(String login)
    {
        var user = _context.Users.FirstOrDefault(u => u.Login == login);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpGet]
    public ActionResult<User> GetAllUsers()
    {
        var users = _context.Users.ToList();
        return Ok(users);
    }
}