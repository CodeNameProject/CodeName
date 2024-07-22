using AutoMapper;
using BLL.Helper;
using BLL.Interface;
using BLL.Models;
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
        await CheckHelper.ModelCheck(id, _unitOfWork.RoomRepository);

        var room = await _unitOfWork.RoomRepository.GetByIdAsync(id);
        var roomMapped = _mapper.Map<RoomModel>(room);

        return roomMapped;
    }

    public async Task AddAsync(RoomModel model)
    {
        CheckHelper.NullCheck(model);

        var room = _mapper.Map<Room>(model);
        await _unitOfWork.RoomRepository.AddAsync(room);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(RoomModel model)
    {
        CheckHelper.NullCheck(model);

        await CheckHelper.ModelCheck(model.Id, _unitOfWork.RoomRepository);

        var room = _mapper.Map<Room>(model);
        _unitOfWork.RoomRepository.Update(room);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(Guid modelId)
    {
        await CheckHelper.ModelCheck(modelId, _unitOfWork.RoomRepository);

        await _unitOfWork.RoomRepository.DeleteByIdAsync(modelId);
        await _unitOfWork.SaveAsync();
    }
}