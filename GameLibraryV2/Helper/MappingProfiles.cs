using GameLibraryV2.Dto;
using GameLibraryV2.Models;
using AutoMapper;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.registry;
using GameLibraryV2.Dto.create;

namespace GameLibraryV2.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {
            //info DTO
            CreateMap<Developer, DeveloperDto>();
            CreateMap<DeveloperDto, Developer>();
            CreateMap<DLC, DLCDto>();
            CreateMap<Friend, FriendDto>();
            CreateMap<Game, GameDto>();
            CreateMap<Genre, GenreDto>();
            CreateMap<Library, LibraryDto>();
            CreateMap<PersonGame, PersonGameDto>();
            CreateMap<Platform, PlatformDto>();
            CreateMap<Publisher, PublisherDto>();
            CreateMap<Review, ReviewDto>();
            CreateMap<Role, RoleDto>();
            CreateMap<Tag, TagDto>();
            CreateMap<User, UserDto>();

            //Small info DTO
            CreateMap<Game, GameListDto>();
            CreateMap<Game, GameSmallDto>();
            CreateMap<Developer, DeveloperSmallDto>();
            CreateMap<Genre, GenreSmallDto>();
            CreateMap<Platform, PlatformSmallDto>();
            CreateMap<Publisher,PublisherSmallDto>();
            CreateMap<Tag, TagSmallDto>();
            CreateMap<User, UserSmallDto>();

            //Create DTO
            CreateMap<DeveloperCreateDto, Developer>();
            CreateMap<GameCreateDto, Game>();
            CreateMap<GenreCreateDto, Genre>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PublisherCreateDto, Publisher>();
            CreateMap<RoleCreateDto, Role>();
            CreateMap<SystemRequirementsMaxCreateDto, SystemRequirementsMax>();
            CreateMap<SystemRequirementsMinCreateDto, SystemRequirementsMin>();
            CreateMap<TagCreateDto, Tag>();
            CreateMap<UserCreateDto, User>();
        }

    }
}
