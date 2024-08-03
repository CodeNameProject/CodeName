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
    private readonly IRoomService _roomService;

    public UserController(IUserService userService, IRoomService roomService)
    {
        _userService = userService;
        _roomService = roomService;
    }

    [HttpDelete("{userid:guid}")]
    public async Task<IActionResult> LogOut(Guid userid)
    {
        try
        {
            var userCount = await _roomService.CheckUserNumberInRoom(userid);

            if (userCount <= 1)
            {
                await _roomService.DeleteRoomByUserId(userid);
            }
            else
            {
                await _userService.DeleteAsync(userid);
            }

            return Ok("Removed successfully..");
        }
        catch (CustomException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{userid:guid}/{newName}")]
    public async Task<IActionResult> ChangeUserName(Guid userid, string newName)
    {
        try
        {
            await _userService.ChangeUserName(userid, newName);
            return Ok();
        }
        catch (CustomException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{userId:guid}")]
    public async Task<IActionResult> SetUserTeamAndRole(Guid userId, [FromQuery] UserRole userRole,
        TeamColor? teamColor)
    {
        try
        {
            await _userService.SetTeamAndRole(userId, userRole, teamColor);
            return Ok();
        }
        catch (CustomException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}