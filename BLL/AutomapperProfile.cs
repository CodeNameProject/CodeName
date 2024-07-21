using AutoMapper;
using BLL.Models;
using DLL.Entities;

namespace BLL;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<Room, RoomModel>()
            .ForMember(dest => dest.Id,r => r.MapFrom(src => src.Id))
            .ForMember(dest => dest.Users,r => r.MapFrom(src => src.Users))
            .ForMember(dest => dest.WordRooms, r => r.MapFrom(src => src.WordRooms))
            .ReverseMap();

        CreateMap<User, UserModel>()
            .ForMember(dest => dest.Id, r => r.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, r => r.MapFrom(src => src.Name))
            .ForMember(dest => dest.Team, r => r.MapFrom(src => src.Team))
            .ForMember(dest => dest.RoomId, r => r.MapFrom(src => src.RoomId))
            .ReverseMap();

        CreateMap<Word, WordModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.WordName, opt => opt.MapFrom(src => src.WordName))
            .ForMember(dest => dest.WordRooms, opt => opt.MapFrom(src => src.WordRooms))
            .ReverseMap();
    }
}
