using AutoMapper;
using BLL.Helper;
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
    private const int WordCount = 25;

    public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    //GetRoom With Id
    
    //AddUser With Name And should back Room
    public async Task<RoomModel> CreateRoomWithUserAsync(string username)
    {
        var room = new Room();

        var user = new User
        {
            Name = username,
            RoomId = room.Id,
            Room = room
        };

        // await _unitOfWork.RoomRepository.AddAsync(room);

        await _unitOfWork.UserRepository.AddAsync(user);

        room = await _unitOfWork.RoomRepository.GetByIdAsync(room.Id);

        var roomModel = _mapper.Map<RoomModel>(room);

        return roomModel;
    }

    private async Task<IEnumerable<WordRoomModel>> GetWordRoomModels(Guid roomId)
    {
        var words = await GetRandomWordsAsync();

        var wordRooms = new List<WordRoomModel>();

        var rand = new Random();
        
        //9 Red , 8 Blue , 1 Bomb , 7 Default

        foreach (var word in words)
        {
            var wordRoom = new WordRoomModel
            {
                // Color = ,
                IsGuessed = false,
                RoomId = roomId,
                WordId = word.Id,
                WordName = word.WordName,
            };

            wordRooms.Add(wordRoom);
        }

        return wordRooms;
    }

    //Random 25 words
    private async Task<IEnumerable<Word>> GetRandomWordsAsync()
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

        // room.Users.Count() >= 4
        
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

    public async Task<RoomModel> ResetGameAsync(UserModel user)
    {
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(user.RoomId);

        room.IsStarted = false;
        
        if (room.WordRooms is null) throw new CustomException("Room Is Null");
        
        foreach (var wr in room.WordRooms)
        {
            await _unitOfWork.WordRoomRepository.DeleteByIdAsync(wr.Id);
        }
        
        
        //Add new Words
        
        _unitOfWork.RoomRepository.Update(room);
        return new RoomModel();
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
}