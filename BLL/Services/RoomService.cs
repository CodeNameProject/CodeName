using AutoMapper;
using BLL.Interface;
using BLL.Models;
using BLL.Validation;
using DLL.Entities;
using DLL.Interface;

namespace BLL.Services;

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<IEnumerable<RoomModel>> GetAllAsync()
    {
        var rooms = await _unitOfWork.RoomRepository.GetAllAsync();
        var roomsMapped = _mapper.Map<IEnumerable<RoomModel>>(rooms);
        return roomsMapped;
    }

    public async Task<RoomModel> GetByIdAsync(Guid id)
    {
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(id);
        var roomMapped = _mapper.Map<RoomModel>(room);
        return roomMapped;
    }

    public async Task AddAsync(RoomModel model)
    {
        var room = _mapper.Map<Room>(model);
        await _unitOfWork.RoomRepository.AddAsync(room);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(RoomModel model)
    {
        var room = _mapper.Map<Room>(model);
        _unitOfWork.RoomRepository.Update(room);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(Guid modelId)
    {
        await _unitOfWork.RoomRepository.DeleteByIdAsync(modelId);
        await _unitOfWork.SaveAsync();
    }
}