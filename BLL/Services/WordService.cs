using AutoMapper;
using BLL.Interface;
using BLL.Models;
using DLL.Entities;
using DLL.Interface;

namespace BLL.Services;

public class WordService : IWordService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public WordService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WordModel>> GetAllAsync()
    {
        var words = await _unitOfWork.WordRepository.GetAllAsync();
        var wordsMapped = _mapper.Map<IEnumerable<WordModel>>(words);
        return wordsMapped;
    }

    public async Task<WordModel> GetByIdAsync(Guid id)
    {
        var word = await _unitOfWork.WordRepository.GetByIdAsync(id);
        var wordMapped = _mapper.Map<WordModel>(word);
        return wordMapped;
    }

    public async Task AddAsync(WordModel model)
    {
        var word = _mapper.Map<Word>(model);
        await _unitOfWork.WordRepository.AddAsync(word);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(WordModel model)
    {
        var word = _mapper.Map<Word>(model);
        _unitOfWork.WordRepository.Update(word);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(Guid modelId)
    {
        await _unitOfWork.WordRepository.DeleteByIdAsync(modelId);
        await _unitOfWork.SaveAsync();
    }
}