using BLL.Interface;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodeNamesAPI.Controllers;

[ApiController]
[Route("user")]
public class TestController : ControllerBase
{
    private readonly IUserService _service;

    public TestController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var users = await _service.GetAllAsync();
        return Ok(users);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] UserModel user)
    {
        await _service.AddAsync(user);
        return Ok("Added Successfully..");
    }

    [HttpGet]
    [Route("exact/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _service.GetByIdAsync(id);
        return Ok(user);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Remove(Guid id)
    {
        await _service.DeleteAsync(id);
        return Ok("Removed Successfully..");
    }
}