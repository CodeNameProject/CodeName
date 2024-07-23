using AutoMapper;
using BLL.Models;
using BLL.Services;
using DLL.Entities;
using DLL.Enums;
using DLL.Interface;
using Moq;
using NUnit.Framework;

namespace BLL.Tests;

public class UserServiceTest
{
    [Test]
    public async Task SetTeamAndRole_WhenTeamColorIsNull_ChangeOnlyRole()
    {
        //Arrange
        var user = new UserModel()
        {
            Id = Guid.NewGuid(),
            Name = "giorgi",
            RoomId = Guid.NewGuid(),
            UserRole = UserRole.Operative
        };
        
        var userRepositoryMock = new Mock<IUserRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var mapperMock = new Mock<IMapper>();
        
        unitOfWorkMock.Setup(x => x.UserRepository).Returns(userRepositoryMock.Object);

        var service = new UserService(unitOfWorkMock.Object, mapperMock.Object);

        var newRole = UserRole.Spymaster;
        //Act
         await service.SetTeamAndRole(user.Id,UserRole.Spymaster,null);

        //Assert
        Assert.That(user.UserRole, Is.EqualTo(newRole));
        
        userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()),Times.Once);
        unitOfWorkMock.Verify(x => x.SaveAsync(),Times.Once);
    }

    [Test]
    public async Task SetTeamAndRole_WhenTeamColorAndRolePassed_ChangeRoleAndColor()
    {
        //Arrange
        var user = new UserModel()
        {
            Id = Guid.NewGuid(),
            Name = "giorgi",
            RoomId = Guid.NewGuid(),
            TeamColor = TeamColor.Blue,
            UserRole = UserRole.Operative
        };
        var userRepositoryMock = new Mock<IUserRepository>();
        var unitOfWorkmock = new Mock<IUnitOfWork>();
        unitOfWorkmock.Setup(rep => rep.UserRepository).Returns(userRepositoryMock.Object);

        var mapperMock = new Mock<IMapper>();

        var service = new UserService(unitOfWorkmock.Object, mapperMock.Object);

        const TeamColor color = TeamColor.Red;
        const UserRole role = UserRole.Spymaster;
        
        //Act
        await service.SetTeamAndRole(user.Id, UserRole.Spymaster, TeamColor.Red);

        //Assert
        Assert.That(user.UserRole,Is.EqualTo(role));
        Assert.That(user.TeamColor,Is.EqualTo(color));
        
        userRepositoryMock.Verify(x => x.Update(It.IsAny<User>()),Times.Once);
        unitOfWorkmock.Verify(x => x.SaveAsync(),Times.Once);
    }

    [Test]
    public async Task ChangeUserName_WhenUserAndNamePassed_ChangesUsersName()
    {
        //Arrange
        var user = new UserModel()
        {
            Id = Guid.NewGuid(),
            Name = "Giorgi",
            RoomId = Guid.NewGuid(),
        };

        var userRepositoryMock = new Mock<IUserRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperMock = new Mock<IMapper>();
        
        unitOfWorkMock.Setup(opt => opt.UserRepository).Returns(userRepositoryMock.Object);

        var service = new UserService(unitOfWorkMock.Object, mapperMock.Object);

        const string name = "luka";
        //Act
        await service.ChangeUserName(user.Id, name);

        //Assert
        Assert.That(user.Name,Is.EqualTo(name));
        
        userRepositoryMock.Verify(opt => opt.Update(It.IsAny<User>()),Times.Once);
        unitOfWorkMock.Verify(opt => opt.SaveAsync(),Times.Once);
    }
}