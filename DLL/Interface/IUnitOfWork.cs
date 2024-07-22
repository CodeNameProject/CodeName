namespace DLL.Interface;

public interface IUnitOfWork
{
    public IRoomRepository RoomRepository { get; }
    public IUserRepository UserRepository { get; }
    public IWordRepository WordRepository { get; }
	public IWordRoomRepository WordRoomRepository { get; }

	Task SaveAsync();   
}