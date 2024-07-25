using BLL.Models;
using DLL.Entities;

namespace BLL.Interface;

public interface IRoomService : ICrud<RoomModel>
{
	Task<RoomModel> AddUserToRoomAsync(Guid roomid, string username);
	Task<RoomModel> CreateRoomWithUserAsync(string username);
	Task<RoomModel> ResetGameAsync(UserModel user);
	Task StartGameAsync(UserModel user);
	Task DeleteByIdAsync(Guid id);
	void ShuffleRoomModel(RoomModel model);
}