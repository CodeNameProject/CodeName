using AutoMapper;
using BLL.Interface;
using BLL.Models;
using BLL.Services;
using BLL.Validation;
using DLL.Data;
using DLL.Entities;
using DLL.Enums;
using DLL.Interface;
using DLL.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace BLL.Tests;

public class RoomServiceTests
{
	private Mock<IUnitOfWork> _unitOfWorkMock;
	private Mock<IRoomRepository> _roomRepositoryMock;
	private Mock<IWordRoomRepository> _wordRoomRepositoryMock;
	private Mock<IWordRepository> _wordRepositoryMock;
	private Mock<IUserRepository> _userRepositoryMock;
	private IRoomService _roomService;

	[SetUp]
    public void SetUp()
    {
        var services = new ServiceCollection();
        services.AddAutoMapper(x => x.AddProfile(new AutomapperProfile()));

		services.AddDbContext<AppDbContext>(opts => opts.UseInMemoryDatabase("inmemory"));

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _roomRepositoryMock = new Mock<IRoomRepository>();
        _wordRoomRepositoryMock = new Mock<IWordRoomRepository>();
        _wordRepositoryMock = new Mock<IWordRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();

		_unitOfWorkMock.Setup(u => u.RoomRepository).Returns(_roomRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.WordRoomRepository).Returns(_wordRoomRepositoryMock.Object);
		_unitOfWorkMock.Setup(u => u.WordRepository).Returns(_wordRepositoryMock.Object);
		_unitOfWorkMock.Setup(u => u.UserRepository).Returns(_userRepositoryMock.Object);


		var mapperExpression = new MapperConfigurationExpression();
        mapperExpression.AddProfile(new AutomapperProfile());
        var mapperConfig = new MapperConfiguration(mapperExpression);
        var mapper = new Mapper(mapperConfig);
        _roomService = new RoomService(_unitOfWorkMock.Object, mapper);
    }

    [Test]
    public async Task CreateRoomWithUserAsync_PassesUserName_ReturnsRoomWithUser()
    {
        var userName = "Test1";
        var room = new Room
        {
            Users = new List<User>()
            {
                new()
                {
                    Name = userName,
                },
            },
        };
        _wordRepositoryMock.Setup(w => w.GetAllAsync()).ReturnsAsync(GetWords());
		_roomRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(room);

        //Act
        var actual = await _roomService.CreateRoomWithUserAsync(userName);
        Assert.That(actual.Users!.Count, Is.EqualTo(1));
		_wordRoomRepositoryMock.Verify(wr => wr.AddAsync(It.IsAny<WordRoom>()), Times.Exactly(25));
    }

	[Test]
	public async Task CreateRoomWithUserAsync_PassesUserName_ReturnsRoomWithWords()
	{
		var services = new ServiceCollection();

		services.AddDbContext<AppDbContext>(opts => opts.UseInMemoryDatabase("memory"));
		services.AddAutoMapper(m => m.AddProfile(new AutomapperProfile()));

		var provider = services.BuildServiceProvider();

		var mapper = provider.GetRequiredService<IMapper>();

		var context = provider.GetRequiredService<AppDbContext>();

		var roomRepository = new RoomRepository(context);
		var userRepository = new UserRepository(context);
		var wordRepository = new WordRepository(context);
		var wordRoomRepository = new WordRoomRepository(context);
		var unitOfWork = new UnitOfWork(wordRoomRepository, roomRepository, userRepository, wordRepository,context);

		foreach(var word in GetWords())
		{
			await unitOfWork.WordRepository.AddAsync(word);
		}
		await unitOfWork.SaveAsync();

		var roomService = new RoomService(unitOfWork, mapper);

		//Arrange
		var userName = "userName1";

		//Act
		
		var room = await roomService.CreateRoomWithUserAsync(userName);

		//Assert

		Assert.That(room.WordRooms, Is.Not.Null);
		Assert.That(room.WordRooms.Count, Is.EqualTo(25));
		Assert.That(room.WordRooms.Where(wr => wr.Color == WordColor.Black).Count, Is.EqualTo(1));
		Assert.That(room.WordRooms.Where(wr => wr.Color == WordColor.Blue).Count, Is.EqualTo(8));
		Assert.That(room.WordRooms.Where(wr => wr.Color == WordColor.Red).Count, Is.EqualTo(9));

		context.Database.EnsureDeleted();
	}

	[Test]
	public async Task AddUserToRoomAsync_WhenCalled_CallsAddAsync()
	{
		//Arrange
		var roomGuid = Guid.NewGuid();
		var room = new Room()
		{
			Id = roomGuid,
		};
		var userName = "testName";

		_roomRepositoryMock.Setup(r => r.GetByIdAsync(roomGuid)).ReturnsAsync(room);

		//Act
		await _roomService.AddUserToRoomAsync(roomGuid, userName);

		//Assert
		_userRepositoryMock.Verify(u => u.AddAsync(It.IsAny<User>()), Times.Once);
		_roomRepositoryMock.Verify(r => r.GetByIdAsync(roomGuid), Times.AtLeastOnce);
		_unitOfWorkMock.Verify(r => r.SaveAsync(), Times.AtLeastOnce);
	}

	[Test]
	public void StartGameAsync_NotEnoughPlayers_Throws()
	{
		//Arrange
		var roomGuid = Guid.NewGuid();
		var user = new UserModel
		{
			RoomId = roomGuid,
		};
		var room = new Room
		{
			Id = roomGuid,
			IsStarted = false,
			Users = new List<User>
			{
				new(),
			}
		};
		_roomRepositoryMock.Setup(r => r.GetByIdAsync(roomGuid)).ReturnsAsync(room);

		//Act & Assert
		Assert.ThrowsAsync<CustomException>(async () => await _roomService.StartGameAsync(user));
	}

	[Test]
	public async Task StartGameAsync_EnoughPlayers_UpdatesRoom()
	{
		//Arrange
		var roomGuid = Guid.NewGuid();
		var user = new UserModel
		{
			RoomId = roomGuid,
		};
		var room = new Room
		{
			Id = roomGuid,
			IsStarted = false,
			Users = new List<User>
			{
				new(),
				new(),
				new(),
				new(),
			}
		};
		_roomRepositoryMock.Setup(r => r.GetByIdAsync(roomGuid)).ReturnsAsync(room);

		//Act
		await _roomService.StartGameAsync(user);

		//Assert
		_roomRepositoryMock.Verify(r => r.GetByIdAsync(roomGuid), Times.Once);
		_roomRepositoryMock.Verify(r => r.Update(room), Times.Once);
		_unitOfWorkMock.Verify(r => r.SaveAsync(), Times.AtLeastOnce);
	}

	[Test]
	public async Task ResetGameAsync_IsStartedSetToTrue_SetsToFalse()
	{
		var roomGuid = Guid.NewGuid();

		var _room = new Room
		{
			Id = roomGuid,
		};

		_wordRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(GetWords());

		_roomRepositoryMock.Setup(r => r.GetByIdAsync(roomGuid)).ReturnsAsync(_room);

		Assert.That(_room.IsStarted, Is.False);
	}

	[Ignore("Words' Getter")]
	private static List<Word> GetWords()
	{
        return new List<Word>
        {
            new()
            {
                WordName = $"Wordname1",
            },
			new()
			{
				WordName = $"Wordname2",
			},
			new()
			{
				WordName = $"Wordname3",
			},
			new()
			{
				WordName = $"Wordname4",
			},
			new()
			{
				WordName = $"Wordname5",
			},
			new()
			{
				WordName = $"Wordname6",
			},
			new()
			{
				WordName = $"Wordname7",
			},
			new()
			{
				WordName = $"Wordname8",
			},
			new()
			{
				WordName = $"Wordname9",
			},
			new()
			{
				WordName = $"Wordname10",
			},
			new()
			{
				WordName = $"Wordname11",
			},
			new()
			{
				WordName = $"Wordname12",
			},
			new()
			{
				WordName = $"Wordname13",
			},
			new()
			{
				WordName = $"Wordname14",
			},
			new()
			{
				WordName = $"Wordname15",
			},
			new()
			{
				WordName = $"Wordname16",
			},
			new()
			{
				WordName = $"Wordname17",
			},
			new()
			{
				WordName = $"Wordname18",
			},
			new()
			{
				WordName = $"Wordname19",
			},
			new()
			{
				WordName = $"Wordname20",
			},
			new()
			{
				WordName = $"Wordname21",
			},
			new()
			{
				WordName = $"Wordname22",
			},
			new()
			{
				WordName = $"Wordname23",
			},
			new()
			{
				WordName = $"Wordname24",
			},
			new()
			{
				WordName = $"Wordname25",
			},
		};
	}
}