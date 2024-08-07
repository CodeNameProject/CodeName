using BLL.Models;
using DAL.Entities;

namespace BLL.Interface;

public interface IRoomService : ICrud<RoomModel>
{
	Task<RoomModel> AddUserToRoomAsync(Guid roomid, string username);
	Task<RoomModel> CreateRoomWithUserAsync(string username);
	Task<RoomModel> ResetGameAsync(UserModel user);
	Task StartGameAsync(UserModel user);
	Task DeleteByIdAsync(Guid id);
	void ShuffleRoomModel(RoomModel model);
	Task DeleteRoomByUserId(Guid userId);
	Task<int> CheckUserNumberInRoom(Guid userId);
}