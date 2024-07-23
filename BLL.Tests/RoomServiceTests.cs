using AutoMapper;
using BLL.Interface;
using BLL.Models;
using BLL.Services;
using DLL.Entities;
using DLL.Enums;
using DLL.Interface;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace BLL.Tests;

public class RoomServiceTests
{
	private Mock<IUnitOfWork> _unitOfWorkMock;
	private Mock<IRoomRepository> _roomRepositoryMock;
	private IRoomService _roomService;

	[SetUp]
    public void SetUp()
    {
        var services = new ServiceCollection();
        services.AddAutoMapper(x => x.AddProfile(new AutomapperProfile()));
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _roomRepositoryMock = new Mock<IRoomRepository>();
        _unitOfWorkMock.Setup(u => u.RoomRepository).Returns(_roomRepositoryMock.Object);
        var mapperExpression = new MapperConfigurationExpression();
        mapperExpression.AddProfile(new AutomapperProfile());
        var mapperConfig = new MapperConfiguration(mapperExpression);
        var mapper = new Mapper(mapperConfig);
        _roomService = new RoomService(_unitOfWorkMock.Object, mapper);
    }

    [Test]
    public async Task CheckUserWord_SameRoomAndSameColor_Returns()
    {
        //Arrange
        var roomGuid = Guid.NewGuid();
        var room = new Room
        {
            Id = roomGuid,
            WordRooms = new List<WordRoom>(),
		};

        var user = new UserModel
        {
            Id = Guid.NewGuid(),
            RoomId = roomGuid,
            TeamColor = TeamColor.Red,
        };

        var wordGuid = Guid.NewGuid();
        var word = new Word
        {
            Id = roomGuid,
            WordName = "Test1",
        };

        var wordRoom = new WordRoom
        {
            WordId = wordGuid,
            RoomId = roomGuid,
            Color = WordColor.Red,
        };

        room.WordRooms.Add(wordRoom);

        _roomRepositoryMock.Setup(r => r.GetByIdAsync(roomGuid)).ReturnsAsync(room);


        //Act

        await _roomService.CheckUserWordAsync(user, wordGuid);

        //Assert

        Assert.That(wordRoom.IsUncovered, Is.True);
    }
}