namespace DLL.Interface;

public interface IUnitOfWork
{
    public IRoomRepository RoomRepository { get; set; }
    public IUserRepository UserRepository { get; set; }
    public IWordRepository WordRepository { get; set; }

    Task SaveAsync();   
}