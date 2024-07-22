using AutoMapper;
using BLL.Helper;
using BLL.Interface;
using BLL.Models;
using BLL.Validation;
using DLL.Entities;
using DLL.Enums;
using DLL.Interface;

namespace BLL.Services;

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
	private const int RedCount = 9;
	private const int BlueCount = 8;
	private const int WordCount = 25;

    public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task CheckUserWord(UserModel user, Guid wordId)
    {
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(user.RoomId);

        if (room.WordRooms is null) throw new CustomException();
        
        var userWords = (room.WordRooms.Where(x => (int)x.Color! == (int)user.TeamColor!)).Select(x => x.WordId);

        var chosenWord = await _unitOfWork.WordRepository.GetByIdAsync(wordId);

        if (userWords.Contains(chosenWord.Id))
        {
            var roomWord = room.WordRooms.FirstOrDefault(x => x.WordId == wordId);

            if (roomWord != null) roomWord.IsUncovered = true;
        }
    }
    
    //GetRoom With Id
    
    //AddUser With Name And should back Room
    public async Task<RoomModel> CreateRoomWithUserAsync(string username)
    {
        var room = new Room();

        await _unitOfWork.RoomRepository.AddAsync(room);

        var user = new User
        {
            Name = username,
            RoomId = room.Id,
        };

        await _unitOfWork.UserRepository.AddAsync(user);

        await AddWordsToRoomAsync(room.Id);

        room = await _unitOfWork.RoomRepository.GetByIdAsync(room.Id);

        var roomModel = _mapper.Map<RoomModel>(room);

        return roomModel;
    }

    //Returns RoomID
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
        
        if (!room.IsStarted && room.Users!.Count >= 4)
        {
            room.IsStarted = true;
        }
        else
        {
            throw new CustomException("User's game has already been started");
        }

        _unitOfWork.RoomRepository.Update(room);
    }

    

    public async Task<RoomModel> ResetGameAsync(UserModel user)
    {
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(user.RoomId);

        room.IsStarted = false;
        
        if (room.WordRooms is null) throw new CustomException("Room Is Null");
        
        foreach (var wr in room.WordRooms)
        {
            await _unitOfWork.WordRoomRepository.DeleteByIdAsync(wr.Id);
        }

		_unitOfWork.RoomRepository.Update(room);

        await AddWordsToRoomAsync(user.RoomId);

        var roomModel = await GetByIdAsync(user.RoomId);

        return roomModel;
    }

    //---------------------////////////
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
	
    private async Task AddWordsToRoomAsync(Guid roomId)
	{
		var words = await GetRandomWordsAsync();

		await AddBombAsync(words, roomId);

		await AddBluesAsync(words, roomId);

		await AddRedsAsync(words, roomId);

		await AddNeutralsAsync(words, roomId);
	}

	private async Task AddNeutralsAsync(List<Word> words, Guid roomId)
	{
		foreach (var word in words)
		{
			var wordRoom = new WordRoom
			{
				IsUncovered = false,
				Color = WordColor.Default,
				RoomId = roomId,
				WordId = word.Id
			};

			await _unitOfWork.WordRoomRepository.AddAsync(wordRoom);
			await _unitOfWork.SaveAsync();

			var wrModel = _mapper.Map<WordRoomModel>(wordRoom);
		}
	}

	private async Task AddRedsAsync(List<Word> words, Guid roomId)
	{
		var rand = new Random();

		for (var i = 0; i < RedCount; i++)
		{
			var randomIndex = rand.Next(words.Count);

			var wordRoom = new WordRoom
			{
				RoomId = roomId,
				WordId = words[randomIndex].Id,
				Color = WordColor.Red,
				IsUncovered = false,
			};

			words.RemoveAt(randomIndex);

			await _unitOfWork.WordRoomRepository.AddAsync(wordRoom);

			await _unitOfWork.SaveAsync();
		}
	}

	private async Task AddBluesAsync(List<Word> words, Guid roomId)
	{
		var rand = new Random();

		for (var i = 0; i < BlueCount; i++)
		{
			var randomIndex = rand.Next(words.Count);

			var wordRoom = new WordRoom
			{
				RoomId = roomId,
				WordId = words[randomIndex].Id,
				Color = WordColor.Blue,
				IsUncovered = false,
			};

			words.RemoveAt(randomIndex);

			await _unitOfWork.WordRoomRepository.AddAsync(wordRoom);

			await _unitOfWork.SaveAsync();
		}
	}

	private async Task AddBombAsync(List<Word> words, Guid roomId)
	{
		var rand = new Random();

		var randomId = rand.Next(words.Count);

		var wordRoom = new WordRoom
		{
			RoomId = roomId,
			WordId = words[randomId].Id,
			Color = WordColor.Black,
			IsUncovered = false,
		};

		words.RemoveAt(randomId);

		await _unitOfWork.WordRoomRepository.AddAsync(wordRoom);

		await _unitOfWork.SaveAsync();
	}

	//Random 25 words
	private async Task<List<Word>> GetRandomWordsAsync()
	{
		var words = (await _unitOfWork.WordRepository.GetAllAsync()).ToList();

		var wordRange = new Dictionary<int, Word>();

		while (wordRange.Count < WordCount)
		{
			var rand = new Random();
			var randomId = rand.Next(words.Count);
			wordRange[randomId] = words[randomId];
		}

		words = wordRange.Values.ToList();

		return words;
	}
}