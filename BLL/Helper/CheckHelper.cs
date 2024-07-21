using BLL.Models;
using BLL.Validation;
using DLL.Data;
using DLL.Entities;
using DLL.Interface;
using DLL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Helper
{
	public static class CheckHelper
	{
		private const string _modelMessage = "model is null";
		private const string _entityMessage = "entity does not exist";

		public static void ModelCheck<T>(Guid id, IRepository<T> repository)  where T: BaseEntity 
		{
			switch (repository)
			{
				case IWordRoomRepository wordRoomRepository:
					{
						var entity = wordRoomRepository.GetByIdAsync(id);
						NullCheck(entity, _entityMessage);

						break;
					}
				case IWordRepository wordRepository:
					{
						var entity = wordRepository.GetByIdAsync(id);
						NullCheck(entity, _entityMessage);

						break;
					}
				case IRoomRepository roomRepository:
					{
						var entity = roomRepository.GetByIdAsync(id);
						NullCheck(entity, _entityMessage);

						break;
					}
				case IUserRepository userRepository:
					{
						var entity = userRepository.GetByIdAsync(id);
						NullCheck(entity, _entityMessage);

						break;
					}
			}
		}

		public static void NullCheck(object model, string action = _modelMessage)
		{
			if (model is null)
			{
				throw new CustomExeption($"{action}: {nameof(model)}");
			}
		}
	}
}
