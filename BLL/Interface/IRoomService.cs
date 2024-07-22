using BLL.Models;
using DLL.Entities;

namespace BLL.Interface;

public interface IRoomService : ICrud<RoomModel>
{
	Task<RoomModel> AddUserToRoom(Guid roomid, string username);
	Task<RoomModel> CreateRoomWithUserAsync(string username);
	Task<RoomModel> ResetGameAsync(UserModel user);
	Task StartGameAsync(UserModel userId);
}