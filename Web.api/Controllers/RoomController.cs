using BLL.Interface;
using BLL.Validation;
using Microsoft.AspNetCore.Mvc;

namespace CodeNamesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IUserService _userService;

        public RoomController(IRoomService roomService, IUserService userService)
        {
            _roomService = roomService;
            _userService = userService;
        }

        [HttpGet("Check")]
        public async Task<IActionResult> CheckOperativeWord([FromQuery]Guid userId,Guid wordId)
        {
            try
            {
                var user = await _userService.GetByIdAsync(userId);
                await _roomService.CheckUserWordAsync(user,wordId);
                return Ok();
            }
            catch (CustomException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("{username}")]
        public async Task<IActionResult> CreateRoomAndUser(string username)
        {
            try
            {
                var room = await _roomService.CreateRoomWithUserAsync(username);
                return Ok(room);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{roomid:guid}/{username}")]
        public async Task<IActionResult> AddUserToRoom(Guid roomid, string username)
        {
            try
            {
                var room = await _roomService.AddUserToRoomAsync(roomid, username);
                return Ok(room);
            }
            catch (CustomException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{userid:guid}")]
        public async Task<IActionResult> StartGame(Guid userId)
        {
            try
            {
                var user = await _userService.GetByIdAsync(userId);
                await _roomService.StartGameAsync(user);
                return Ok();
            }
            catch (CustomException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{userId:guid}")]
        public async Task<IActionResult> ResetGame(Guid userId)
        {
            try
            {
                var user = await _userService.GetByIdAsync(userId);
                await _roomService.ResetGameAsync(user);
                return Ok();
            }
            catch (CustomException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}