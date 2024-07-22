using AutoMapper;
using BLL.Helper;
using BLL.Interface;
using BLL.Models;
using DLL.Entities;
using DLL.Interface;

namespace BLL.Services;

public class WordRoomService : IWordRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public WordRoomService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task AddAsync(WordRoomModel model)
    {
        CheckHelper.NullCheck(model);

        var word = _mapper.Map<WordRoom>(model);
        await _unitOfWork.WordRoomRepository.AddAsync(word);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(Guid modelId)
    {
        await CheckHelper.ModelCheck(modelId, _unitOfWork.WordRoomRepository);

        await _unitOfWork.WordRoomRepository.DeleteByIdAsync(modelId);
        await _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<WordRoomModel>> GetAllAsync()
    {
        var words = await _unitOfWork.WordRoomRepository.GetAllAsync();
        var wordsMapped = _mapper.Map<IEnumerable<WordRoomModel>>(words);

        return wordsMapped;
    }

    public async Task<WordRoomModel> GetByIdAsync(Guid id)
    {
        await CheckHelper.ModelCheck(id, _unitOfWork.WordRoomRepository);

        var word = await _unitOfWork.WordRoomRepository.GetByIdAsync(id);
        var wordMapped = _mapper.Map<WordRoomModel>(word);

        return wordMapped;
    }

    public async Task UpdateAsync(WordRoomModel model)
    {
        CheckHelper.NullCheck(model);
        await CheckHelper.ModelCheck(model.Id, _unitOfWork.WordRoomRepository);

        var word = _mapper.Map<WordRoom>(model);
        _unitOfWork.WordRoomRepository.Update(word);
        await _unitOfWork.SaveAsync();
    }
}