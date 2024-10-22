using BLL.Models;
using DAL.Entities;
using DAL.Enums;

namespace BLL.Interface;

public interface IUserService : ICrud<UserModel>
{
    Task ChangeUserName(Guid userId,string newName);
    Task SetTeamAndRole(Guid userId, UserRole userRole, TeamColor? teamColor);
    Task DeleteAsync(Guid modelId);
}