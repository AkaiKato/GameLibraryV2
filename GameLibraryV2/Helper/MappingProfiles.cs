using AutoMapper;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.registry;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.Create;
using GameLibraryV2.Dto.create;
using GameLibraryV2.Models.Common;

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
            CreateMap<GameDto, Game>();
            CreateMap<Genre, GenreDto>();
            CreateMap<PersonGame, PersonGameDto>();
            CreateMap<Platform, PlatformDto>();
            CreateMap<Publisher, PublisherDto>();
            CreateMap<Review, ReviewDto>();
            CreateMap<Role, RoleDto>();
            CreateMap<Tag, TagDto>();
            CreateMap<User, UserDto>();

            //Small info DTO
            CreateMap<Game, GameSmallListDto>();
            CreateMap<Game, GameSmallDto>();
            CreateMap<Developer, DeveloperSmallDto>();
            CreateMap<Genre, GenreSmallDto>();
            CreateMap<Platform, PlatformSmallDto>();
            CreateMap<Publisher,PublisherSmallDto>();
            CreateMap<Tag, TagSmallDto>();
            CreateMap<User, UserSmallDto>();

            //Create DTO
            CreateMap<AgeRatingCreateDto, AgeRating>();
            CreateMap<DeveloperCreateDto, Developer>();
            CreateMap<GameCreateDto, Game>();
            CreateMap<int, AgeRating>();
            CreateMap<int, Developer>();
            CreateMap<int, Publisher>();
            CreateMap<int, Platform>();
            CreateMap<int, Tag>();
            CreateMap<int, Genre>();
            CreateMap<SystemRequirementsCreateDto,  SystemRequirements>();
            CreateMap<GenreCreateDto, Genre>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PublisherCreateDto, Publisher>();
            CreateMap<ReviewCreateDto, Review>();
            CreateMap<RoleCreateDto, Role>();
            CreateMap<TagCreateDto, Tag>();
            CreateMap<UserCreateDto, User>();


            CreateMap<GameUpdate, Game>();
            CreateMap<UserUpdate, User>();
            CreateMap<DeveloperSmallDto, Developer>();
            CreateMap<GenreSmallDto, Genre>();
            CreateMap<PlatformSmallDto, Platform>();
            CreateMap<PublisherSmallDto, Publisher>();
            CreateMap<TagSmallDto, Tag>();
        }

    }
}
