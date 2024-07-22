using BLL.Models;
using DLL.Entities;
using DLL.Enums;

namespace BLL.Interface;

public interface IUserService : ICrud<UserModel>
{
    Task ChangeUserName(UserModel user,string newName);
    Task SetTeamAndRole(UserModel user, UserRole userRole, TeamColor? teamColor);
}