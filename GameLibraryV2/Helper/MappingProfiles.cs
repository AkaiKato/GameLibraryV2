using GameLibraryV2.Dto;
using GameLibraryV2.Models;
using AutoMapper;
using GameLibraryV2.Dto.smallInfo;

namespace GameLibraryV2.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {
            CreateMap<Developer, DeveloperDto>();
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

            CreateMap<Game, GameListDto>();
            CreateMap<Game, GameSmallDto>();
            CreateMap<Developer, DeveloperSmallDto>();
            CreateMap<Genre, GenreSmallDto>();
            CreateMap<Platform, PlatformSmallDto>();
            CreateMap<Publisher,PublisherSmallDto>();
            CreateMap<Tag, TagSmallDto>();
            CreateMap<User, UserSmallDto>();
        }

    }
}
