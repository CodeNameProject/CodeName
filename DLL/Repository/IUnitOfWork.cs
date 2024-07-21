using DLL.Interface;

namespace DLL.Repository;

public interface IUnitOfWork
{
    public IRoomRepository RoomRepository { get; set; }
    public IUserRepository UserRepository { get; set; }
    public IWordRepository WordRepository { get; set; }
}