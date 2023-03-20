using GameLibraryV2.Dto;
using GameLibraryV2.Models;
using AutoMapper;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.registry;

namespace GameLibraryV2.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {
            CreateMap<Developer, DeveloperDto>();
            CreateMap<DeveloperDto, Developer>();
            CreateMap<DLC, DLCDto>();
            CreateMap<Friend, FriendDto>();
            CreateMap<Game, GameDto>();
            CreateMap<Genre, GenreDto>();
            CreateMap<GenreDto, Genre>();
            CreateMap<Library, LibraryDto>();
            CreateMap<LibraryDto, Library>();
            CreateMap<PersonGame, PersonGameDto>();
            CreateMap<Platform, PlatformDto>();
            CreateMap<PlatformDto, Platform>();
            CreateMap<Publisher, PublisherDto>();
            CreateMap<PublisherDto, Publisher>();
            CreateMap<Review, ReviewDto>();
            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();
            CreateMap<Tag, TagDto>();
            CreateMap<TagDto, Tag>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Game, GameListDto>();
            CreateMap<Game, GameSmallDto>();
            CreateMap<Developer, DeveloperSmallDto>();
            CreateMap<Genre, GenreSmallDto>();
            CreateMap<Platform, PlatformSmallDto>();
            CreateMap<Publisher,PublisherSmallDto>();
            CreateMap<Tag, TagSmallDto>();
            CreateMap<User, UserSmallDto>();

            CreateMap<UserCreateDto, User>();
        }

    }
}
