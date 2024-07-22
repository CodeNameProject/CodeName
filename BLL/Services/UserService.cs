using AutoMapper;
using BLL.Helper;
using BLL.Interface;
using BLL.Models;
using DLL.Entities;
using DLL.Enums;
using DLL.Interface;

namespace BLL.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task SetTeamAndRole(UserModel user, UserRole userRole, TeamColor? teamColor)
    {
        //Front should decide if spymaster can change role
        if (teamColor is null)
        {
            user.UserRole = userRole;
        }
        else
        {
            user.UserRole = userRole;
            user.TeamColor = teamColor;
        }

        var userMapped = _mapper.Map<User>(user);
        _unitOfWork.UserRepository.Update(userMapped);
        await _unitOfWork.SaveAsync();
    }
    
    public async Task ChangeUserName(UserModel user, string newName)
    {
        user.Name = newName;
        
        var userMapped = _mapper.Map<User>(user);
        
        _unitOfWork.UserRepository.Update(userMapped);
        await _unitOfWork.SaveAsync();
    }
    
    public async Task<IEnumerable<UserModel>> GetAllAsync()
    {
        var users = await _unitOfWork.UserRepository.GetAllAsync();
        var usersMapped = _mapper.Map<IEnumerable<UserModel>>(users);
        return usersMapped;
    }

    public async Task<UserModel> GetByIdAsync(Guid id)
    {
        await CheckHelper.ModelCheckAsync(id, _unitOfWork.UserRepository);

        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        var userMapped = _mapper.Map<UserModel>(user);

        return userMapped;
    }

    public async Task AddAsync(UserModel model)
    {
        CheckHelper.NullCheck(model);

        var user = _mapper.Map<User>(model);
        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(UserModel model)
    {
        CheckHelper.NullCheck(model);
        await CheckHelper.ModelCheckAsync(model.Id, _unitOfWork.UserRepository);

        var user = _mapper.Map<User>(model);
        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(Guid modelId)
    {
        await CheckHelper.ModelCheckAsync(modelId, _unitOfWork.UserRepository);

        await _unitOfWork.UserRepository.DeleteByIdAsync(modelId);
        await _unitOfWork.SaveAsync();
    }


   
}