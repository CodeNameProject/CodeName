using BLL.Interface;
using BLL.Models;
using BLL.Services;
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

		[HttpPost("{username}")]
		public async Task<IActionResult> CreateRoomAndUser(string username)
		{
			var room = await _roomService.CreateRoomWithUserAsync(username);

			return Ok(room);
		}


		[HttpPost("{roomid:guid}/{username}")]
		public async Task<IActionResult> CreateRoomAndUser(Guid roomid, string username)
		{
			RoomModel room;
			try
			{
				room = await _roomService.AddUserToRoom(roomid, username);
			}
			catch (CustomException ex)
			{
				return BadRequest(ex.Message);
			}

			return Ok(room);
		}

		[HttpPatch("{userid:guid}")]
		public async Task<IActionResult> StartGame(Guid userId)
		{
			try
			{
				var user = await _userService.GetByIdAsync(userId);
				await _roomService.StartGameAsync(user);
			}
			catch (CustomException ex)
			{
				return BadRequest(ex.Message);
			}

			return Ok();
		}

		[HttpPut("{userId:guid}")]
		public async Task<IActionResult> ResetGame(Guid userId)
		{
			try
			{
				var user = await _userService.GetByIdAsync(userId);
				await _roomService.ResetGameAsync(user);
			}
			catch (CustomException ex)
			{
				return BadRequest(ex.Message);
			}

			return Ok();
		}
	}
}
