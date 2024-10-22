using DAL.Interface;
using DAL.Repository;

namespace DAL.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
	private readonly IWordRoomRepository _wordRoomRepository;
	private readonly IRoomRepository _roomRepository;
	private readonly IUserRepository _userRepository;
	private readonly IWordRepository _wordRepository;

	public UnitOfWork(IWordRoomRepository wordRoomRepository,
                        IRoomRepository roomRepository,
                        IUserRepository userRepository,
						IWordRepository wordRepository,
						AppDbContext context)
    {
		_wordRoomRepository = wordRoomRepository;
		_roomRepository = roomRepository;
		_userRepository = userRepository;
		_wordRepository = wordRepository;
		_context = context;
	}

	public IRoomRepository RoomRepository => _roomRepository;
	public IUserRepository UserRepository => _userRepository;
	public IWordRepository WordRepository => _wordRepository;
	public IWordRoomRepository WordRoomRepository => _wordRoomRepository;

	public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}