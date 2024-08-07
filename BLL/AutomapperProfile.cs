using AutoMapper;
using BLL.Models;
using DAL.Entities;

namespace BLL;

public class AutomapperProfile : Profile
{
	public AutomapperProfile()
	{
		CreateMap<Room, RoomModel>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users))
			.ForMember(dest => dest.WordRooms, opt => opt.MapFrom(src => src.WordRooms))
			.ForMember(dest => dest.IsStarted, opt => opt.MapFrom(src => src.IsStarted))
			.ReverseMap();

		CreateMap<User, UserModel>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId))
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.TeamColor, opt => opt.MapFrom(src => src.Team))
			.ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole))
			.ReverseMap();

		CreateMap<Word, WordModel>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.WordName, opt => opt.MapFrom(src => src.WordName))
			.ReverseMap();

		CreateMap<WordRoom, WordRoomModel>()
			.ForMember(dest => dest.WordId, opt => opt.MapFrom(src => src.WordId))
			.ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.RoomId))
			.ForMember(dest => dest.WordName, opt => opt.MapFrom(src => src.Word.WordName))
			.ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color))
			.ForMember(dest => dest.IsGuessed, opt => opt.MapFrom(src => src.IsUncovered))
			.ReverseMap();
	}
}
