using AutoMapper;
using BLL.Helper;
using BLL.Interface;
using BLL.Models;
using BLL.Validation;
using DLL.Entities;
using DLL.Enums;
using DLL.Interface;
using System.Drawing;

namespace BLL.Services;

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
	private readonly int _wordCount = 25;

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
        await CheckHelper.ModelCheckAsync(id, _unitOfWork.RoomRepository);

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

        await CheckHelper.ModelCheckAsync(model.Id, _unitOfWork.RoomRepository);

        var room = _mapper.Map<Room>(model);
        _unitOfWork.RoomRepository.Update(room);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(Guid modelId)
    {
        await CheckHelper.ModelCheckAsync(modelId, _unitOfWork.RoomRepository);

        await _unitOfWork.RoomRepository.DeleteByIdAsync(modelId);
        await _unitOfWork.SaveAsync();
    }

	public async Task<RoomModel> CreateRoomWithUserAsync(string username) //////////////// ADD WORDS
	{
        var room = new Room();

        await _unitOfWork.RoomRepository.AddAsync(room);

        var user = new User
        {
            Name = username,
            RoomId = room.Id,
        };

        await _unitOfWork.UserRepository.AddAsync(user);

        room = await _unitOfWork.RoomRepository.GetByIdAsync(room.Id);

        var roomModel = _mapper.Map<RoomModel>(room);

        return roomModel;
	}

	public async Task<RoomModel> AddUserToRoom(Guid roomId, string username)
	{
        await CheckHelper.ModelCheckAsync(roomId, _unitOfWork.RoomRepository);

		var user = new User
		{
			Name = username,
			RoomId = roomId,
		};

		await _unitOfWork.UserRepository.AddAsync(user);

		var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomId);

		var roomModel = _mapper.Map<RoomModel>(room);

		return roomModel;
	}

	public async Task StartGameAsync(UserModel user)
	{
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(user.RoomId);

        if (!room.IsStarted)
        {
            room.IsStarted = true;
        }
        else
        {
            throw new CustomException("User's game has already been started");
        }
        
        _unitOfWork.RoomRepository.Update(room);
	}

    private async Task<IEnumerable<WordRoomModel>> GetWordRoomModels(Guid roomId)
    {
        var words = await GetRandomWordsAsync();

        var wordRooms = new List<WordRoomModel>();

        var rand = new Random();

        foreach (var word in words)
        {
            var wordRoom = new WordRoomModel
            {
                Color = (WordColor)Enum.ToObject(typeof(WordColor), rand.Next(1, 4)),
                IsGuessed = false,
                RoomId = roomId,
                WordId = word.Id,
                WordName = word.WordName,
            };

            wordRooms.Add(wordRoom);
        }
        return wordRooms;
    }

	private async Task<IEnumerable<Word>> GetRandomWordsAsync()
	{
        var words = (await _unitOfWork.WordRepository.GetAllAsync()).ToList();

        var wordRange = new Dictionary<int, Word>();

		while (wordRange.Count < _wordCount)
        {
            var rand = new Random();
            var randomId = rand.Next(words.Count);
            wordRange[randomId] = words[randomId];
        }
        
        words = wordRange.Values.ToList();

        return words;
	}

	public async Task<RoomModel> ResetGameAsync(UserModel user)
	{
		var room = await _unitOfWork.RoomRepository.GetByIdAsync(user.RoomId);

		room.IsStarted = false;

        foreach(var wr in room.WordRooms)
        {
            await _unitOfWork.WordRoomRepository.DeleteByIdAsync(wr.RoomId);
        }



		_unitOfWork.RoomRepository.Update(room);
	}
}