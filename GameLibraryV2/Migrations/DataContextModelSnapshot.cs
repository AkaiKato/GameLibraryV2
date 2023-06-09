﻿// <auto-generated />
using System;
using GameLibraryV2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GameLibraryV2.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DeveloperGame", b =>
                {
                    b.Property<int>("DeveloperGamesId")
                        .HasColumnType("integer");

                    b.Property<int>("DevelopersId")
                        .HasColumnType("integer");

                    b.HasKey("DeveloperGamesId", "DevelopersId");

                    b.HasIndex("DevelopersId");

                    b.ToTable("DeveloperGame");
                });

            modelBuilder.Entity("GameGenre", b =>
                {
                    b.Property<int>("GenreGamesId")
                        .HasColumnType("integer");

                    b.Property<int>("GenresId")
                        .HasColumnType("integer");

                    b.HasKey("GenreGamesId", "GenresId");

                    b.HasIndex("GenresId");

                    b.ToTable("GameGenre");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.AgeRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AgeRating");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.DLC", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DLCGameId")
                        .HasColumnType("integer");

                    b.Property<int>("ParentGameId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DLCGameId");

                    b.HasIndex("ParentGameId");

                    b.ToTable("DLCs");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.Developer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("MiniPicturePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PicturePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Developers");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.Friend", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FrienduId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FrienduId");

                    b.HasIndex("UserId");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AgeRatingId")
                        .HasColumnType("integer");

                    b.Property<double?>("AveragePlayTime")
                        .HasColumnType("double precision");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("NSFW")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ParentGameId")
                        .HasColumnType("integer");

                    b.Property<string>("PicturePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RatingId")
                        .HasColumnType("integer");

                    b.Property<DateOnly>("ReleaseDate")
                        .HasColumnType("date");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AgeRatingId");

                    b.HasIndex("ParentGameId");

                    b.HasIndex("RatingId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.PersonGame", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<bool>("Favourite")
                        .HasColumnType("boolean");

                    b.Property<int>("GameId")
                        .HasColumnType("integer");

                    b.Property<string>("List")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("PlayedPlatformId")
                        .HasColumnType("integer");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayedPlatformId");

                    b.HasIndex("UserId");

                    b.ToTable("PersonGames");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.Platform", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Platforms");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.Publisher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("MiniPicturePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PicturePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("NumberOfEight")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfFive")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfFour")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfNine")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfOne")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfSeven")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfSix")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfTen")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfThree")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfTwo")
                        .HasColumnType("integer");

                    b.Property<double>("TotalRating")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("GameId")
                        .HasColumnType("integer");

                    b.Property<DateOnly>("PublishDate")
                        .HasColumnType("date");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.Property<int>("ReviewRating")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("UserId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.SystemRequirements", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Additional")
                        .HasColumnType("text");

                    b.Property<string>("DirectX")
                        .HasColumnType("text");

                    b.Property<string>("Ethernet")
                        .HasColumnType("text");

                    b.Property<int?>("GameId")
                        .HasColumnType("integer");

                    b.Property<string>("HardDriveSpace")
                        .HasColumnType("text");

                    b.Property<string>("OC")
                        .HasColumnType("text");

                    b.Property<string>("Processor")
                        .HasColumnType("text");

                    b.Property<string>("RAM")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("VideoCard")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("SystemRequirements");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PicturePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateOnly>("RegistrationdDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("TokenCreated")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("TokenExpires")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GamePlatform", b =>
                {
                    b.Property<int>("PlatformGamesId")
                        .HasColumnType("integer");

                    b.Property<int>("PlatformsId")
                        .HasColumnType("integer");

                    b.HasKey("PlatformGamesId", "PlatformsId");

                    b.HasIndex("PlatformsId");

                    b.ToTable("GamePlatform");
                });

            modelBuilder.Entity("GamePublisher", b =>
                {
                    b.Property<int>("PublisherGamesId")
                        .HasColumnType("integer");

                    b.Property<int>("PublishersId")
                        .HasColumnType("integer");

                    b.HasKey("PublisherGamesId", "PublishersId");

                    b.HasIndex("PublishersId");

                    b.ToTable("GamePublisher");
                });

            modelBuilder.Entity("GameTag", b =>
                {
                    b.Property<int>("TagsGamesId")
                        .HasColumnType("integer");

                    b.Property<int>("TagsId")
                        .HasColumnType("integer");

                    b.HasKey("TagsGamesId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("GameTag");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<int>("RoleUsersId")
                        .HasColumnType("integer");

                    b.Property<int>("UserRolesId")
                        .HasColumnType("integer");

                    b.HasKey("RoleUsersId", "UserRolesId");

                    b.HasIndex("UserRolesId");

                    b.ToTable("RoleUser");
                });

            modelBuilder.Entity("DeveloperGame", b =>
                {
                    b.HasOne("GameLibraryV2.Models.Common.Game", null)
                        .WithMany()
                        .HasForeignKey("DeveloperGamesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameLibraryV2.Models.Common.Developer", null)
                        .WithMany()
                        .HasForeignKey("DevelopersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GameGenre", b =>
                {
                    b.HasOne("GameLibraryV2.Models.Common.Game", null)
                        .WithMany()
                        .HasForeignKey("GenreGamesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameLibraryV2.Models.Common.Genre", null)
                        .WithMany()
                        .HasForeignKey("GenresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.DLC", b =>
                {
                    b.HasOne("GameLibraryV2.Models.Common.Game", "DLCGame")
                        .WithMany()
                        .HasForeignKey("DLCGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameLibraryV2.Models.Common.Game", "ParentGame")
                        .WithMany("DLCs")
                        .HasForeignKey("ParentGameId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("DLCGame");

                    b.Navigation("ParentGame");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.Friend", b =>
                {
                    b.HasOne("GameLibraryV2.Models.Common.User", "Friendu")
                        .WithMany()
                        .HasForeignKey("FrienduId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameLibraryV2.Models.Common.User", "User")
                        .WithMany("UserFriends")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Friendu");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.Game", b =>
                {
                    b.HasOne("GameLibraryV2.Models.Common.AgeRating", "AgeRating")
                        .WithMany()
                        .HasForeignKey("AgeRatingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameLibraryV2.Models.Common.Game", "ParentGame")
                        .WithMany()
                        .HasForeignKey("ParentGameId");

                    b.HasOne("GameLibraryV2.Models.Common.Rating", "Rating")
                        .WithMany()
                        .HasForeignKey("RatingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AgeRating");

                    b.Navigation("ParentGame");

                    b.Navigation("Rating");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.PersonGame", b =>
                {
                    b.HasOne("GameLibraryV2.Models.Common.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameLibraryV2.Models.Common.Platform", "PlayedPlatform")
                        .WithMany()
                        .HasForeignKey("PlayedPlatformId");

                    b.HasOne("GameLibraryV2.Models.Common.User", "User")
                        .WithMany("UserGames")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("PlayedPlatform");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.Review", b =>
                {
                    b.HasOne("GameLibraryV2.Models.Common.Game", "Game")
                        .WithMany("Reviews")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameLibraryV2.Models.Common.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.SystemRequirements", b =>
                {
                    b.HasOne("GameLibraryV2.Models.Common.Game", null)
                        .WithMany("SystemRequirements")
                        .HasForeignKey("GameId");
                });

            modelBuilder.Entity("GamePlatform", b =>
                {
                    b.HasOne("GameLibraryV2.Models.Common.Game", null)
                        .WithMany()
                        .HasForeignKey("PlatformGamesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameLibraryV2.Models.Common.Platform", null)
                        .WithMany()
                        .HasForeignKey("PlatformsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GamePublisher", b =>
                {
                    b.HasOne("GameLibraryV2.Models.Common.Game", null)
                        .WithMany()
                        .HasForeignKey("PublisherGamesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameLibraryV2.Models.Common.Publisher", null)
                        .WithMany()
                        .HasForeignKey("PublishersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GameTag", b =>
                {
                    b.HasOne("GameLibraryV2.Models.Common.Game", null)
                        .WithMany()
                        .HasForeignKey("TagsGamesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameLibraryV2.Models.Common.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("GameLibraryV2.Models.Common.User", null)
                        .WithMany()
                        .HasForeignKey("RoleUsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameLibraryV2.Models.Common.Role", null)
                        .WithMany()
                        .HasForeignKey("UserRolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.Game", b =>
                {
                    b.Navigation("DLCs");

                    b.Navigation("Reviews");

                    b.Navigation("SystemRequirements");
                });

            modelBuilder.Entity("GameLibraryV2.Models.Common.User", b =>
                {
                    b.Navigation("UserFriends");

                    b.Navigation("UserGames");
                });
#pragma warning restore 612, 618
        }
    }
}
