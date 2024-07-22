using BLL.Validation;
using DLL.Entities;
using DLL.Interface;


namespace BLL.Helper
{
	public static class CheckHelper
	{
		private const string ModelMessage = "model is null";
		private const string EntityMessage = "entity does not exist";

		public static async Task ModelCheckAsync<T>(Guid id, IRepository<T> repository)  where T: BaseEntity 
		{
			switch (repository)
			{
				case IWordRoomRepository wordRoomRepository:
					{
						var entity = await wordRoomRepository.GetByIdAsync(id);
						NullCheck(entity, EntityMessage);

						break;
					}
				case IWordRepository wordRepository:
					{
						var entity = await wordRepository.GetByIdAsync(id);
						NullCheck(entity, EntityMessage);

						break;
					}
				case IRoomRepository roomRepository:
					{
						var entity = await roomRepository.GetByIdAsync(id);
						NullCheck(entity, EntityMessage);

						break;
					}
				case IUserRepository userRepository:
					{
						var entity = await userRepository.GetByIdAsync(id);
						NullCheck(entity, EntityMessage);

						break;
					}
			}
		}

		public static void NullCheck(object model, string action = ModelMessage)
		{
			if (model is null)
			{
				throw new CustomException($"{action}: {nameof(model)}");
			}
		}
	}
}
