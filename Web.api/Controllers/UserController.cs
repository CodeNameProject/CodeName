using BLL.Interface;
using BLL.Validation;
using DLL.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CodeNamesAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPatch("{userid:guid}/{newName}")]
    public async Task<IActionResult> ChangeUserName(Guid userid, string newName)
    {
        try
        {
            var user = await _userService.GetByIdAsync(userid);
            await _userService.ChangeUserName(user, newName);
            return Ok();
        }
        catch (CustomException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{userId:guid}")]
    public async Task<IActionResult> SetUserTeamAndRole(Guid userId,[FromQuery]UserRole userRole,TeamColor? teamColor)
    {
        try
        {
            var user = await _userService.GetByIdAsync(userId);
            await _userService.SetTeamAndRole(user,userRole,teamColor);
            return Ok();
        }
        catch (CustomException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}