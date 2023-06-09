﻿using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.Create;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Helper;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models.Common;
using GameLibraryV2.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using static GameLibraryV2.Helper.Enums;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonGameController : Controller
    {
        private readonly IPersonGamesRepository personGameRepository;
        private readonly IGameRepository gameRepository;
        private readonly IUserRepository userRepository;
        private readonly IPlatformRepository platformRepository;
        private readonly IReviewRepository reviewRepository;
        private readonly IRatingRepository ratingRepository;
        private readonly IMapper mapper;

        public PersonGameController(IPersonGamesRepository _personGameRepository,
            IGameRepository _gameRepository, IUserRepository _userRepository,
            IPlatformRepository _platformRepository, IReviewRepository _reviewRepository,
            IRatingRepository _ratingRepository, IMapper _mapper)
        {
            personGameRepository = _personGameRepository;
            gameRepository = _gameRepository;
            userRepository = _userRepository;
            platformRepository = _platformRepository;
            reviewRepository = _reviewRepository;
            ratingRepository = _ratingRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Return all Person Games
        /// </summary>
        /// <returns></returns>
        [HttpGet("{userId}/persongames")]
        public async Task<IActionResult> GetPersonGames(int userId)
        {
            if(!await userRepository.UserExistsByIdAsync(userId))
                return NotFound($"Not found user with such id {userId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var PersonGames = mapper.Map<List<PersonGameDto>>(await personGameRepository.PersonGamesAsync(userId));

            return Ok(PersonGames);
        }

        /// <summary>
        /// Return all Person Games By List
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpGet("{userId}/persongamesbylist")]
        public async Task<IActionResult> GetPersonGamesByList(int userId, string list) 
        {
            if(!await userRepository.UserExistsByIdAsync(userId))
                return NotFound($"Not found user with such id {userId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var PersonGamesByList = mapper.Map<List<PersonGameDto>>(await personGameRepository.PersonGamesByListAsync(userId, list));

            return Ok(PersonGamesByList);
        }

        [HttpGet("userHaveThisPersonGame")]
        public async Task<IActionResult> UserHaveThisPersonGame(int userId, int gameId)
        {
            if (!await personGameRepository.PersonGameExistsAsync(userId, gameId))
                return NotFound($"Not found in person Game");

            var PersonGame = mapper.Map<PersonGameDto>(await personGameRepository.GetPersonGameByUserIdAndGameIdAsync(userId, gameId));

            return Ok(PersonGame);
        }

        /// <summary>
        /// Add Person Game to User
        /// </summary>
        /// <param name="personGameCreate"></param>
        /// <returns></returns>
        [Authorize(Roles = "user")]
        [HttpPost("addPersonGame")]
        public async Task<IActionResult> AddPersonGame([FromBody] PersonGameCreate personGameCreate)
        {
            if(personGameCreate == null)
                return BadRequest(ModelState);

            if (!await userRepository.UserExistsByIdAsync(personGameCreate.UserId))
                return NotFound($"Not found user with such id {personGameCreate.UserId}");

            if (!await userRepository.UserRefreshTokenValid(personGameCreate.UserId, Request.Cookies["refreshToken"]))
                return BadRequest("Invalid Token");

            if (!await gameRepository.GameExistsAsync(personGameCreate.GameId))
                return NotFound($"Not found game with such id {personGameCreate.GameId}");

            personGameCreate.List = personGameCreate.List;
            
            if(!Enum.GetNames(typeof(List)).Contains(personGameCreate.List.Trim().ToLower()))
               return BadRequest("Unsupported list");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var personGame = new PersonGame() 
            {
                User = await userRepository.GetUserByIdAsync(personGameCreate.UserId),
                Game = await gameRepository.GetGameByIdAsync(personGameCreate.GameId),
                List = personGameCreate.List,
            };

            personGameRepository.CreatePersonGame(personGame);
            await personGameRepository.SavePersonGameAsync();

            return Ok("Successfully added");
        }

        /// <summary>
        /// Update Person Game
        /// </summary>
        /// <param name="personGameUpdate"></param>
        /// <returns></returns>
        [Authorize(Roles = "user")]
        [HttpPut("updatePersonGame")]
        public async Task<IActionResult> UpdatePersonGame([FromBody] PersonGameUpdate personGameUpdate)
        {
            if (personGameUpdate == null)
                return BadRequest(ModelState);

            if (!await personGameRepository.PersonGameExistsAsync(personGameUpdate.Id))
                return NotFound($"Not found persongame with such id {personGameUpdate.Id}");

            if (!Enum.GetNames(typeof(Enums.List)).Contains(personGameUpdate.List.Trim().ToLower()))
                return BadRequest("Unsupported list");

            if ((personGameUpdate.Score < 1 || personGameUpdate.Score > 10) && personGameUpdate.Score != -1)
                return BadRequest("Wrong Score");   

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var personGame = await personGameRepository.GetPersonGameByIdAsync(personGameUpdate.Id);

            if(personGame.Score != personGameUpdate.Score && await reviewRepository.ReviewExistsAsync(personGame.User.Id, personGame.Game.Id))
            {
                var review = await reviewRepository.GetReviewByUserIdAndGameIdAsync(personGame.User.Id, personGame.Game.Id);
                review.Rating = personGameUpdate.Score;
                reviewRepository.UpdateReview(review);
                await reviewRepository.SaveReviewAsync();
            }

            if(personGame.Score != personGameUpdate.Score)
            {
                ratingRepository.Remove(personGame.Game.Rating, personGame.Score%10);
                ratingRepository.Add(personGame.Game.Rating, personGameUpdate.Score%10);
            }

            personGame.Score = personGameUpdate.Score;
            personGame.Comment = personGameUpdate.Comment;
            personGame.List = personGameUpdate.List.Trim().ToLower();

            personGame.PlayedPlatform = await platformRepository.GetPlatformByIdAsync(personGameUpdate.PlayedPlatform);

            personGame.Favourite = personGameUpdate.Favourite;

            personGameRepository.UpdatePersonGame(personGame);
            await personGameRepository.SavePersonGameAsync();

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Delete Person Game
        /// </summary>
        /// <param name="personGameDelete"></param>
        /// <returns></returns>
        [Authorize(Roles = "user")]
        [HttpDelete("deletePersonGame")]
        public async Task<IActionResult> DeletePersonGame([FromQuery] Guid personGameDelete)
        {
            if (!await personGameRepository.PersonGameExistsAsync(personGameDelete))
                return NotFound($"Not found personGame with such id {personGameDelete}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var personGame = await personGameRepository.GetPersonGameByIdAsync(personGameDelete);

            if(personGame.Score != -1)
                ratingRepository.Remove(personGame.Game.Rating, personGame.Score % 10);

            if (await reviewRepository.ReviewExistsAsync(personGame.User.Id, personGame.Game.Id))
            {
                var review = await reviewRepository.GetReviewByUserIdAndGameIdAsync(personGame.User.Id, personGame.Game.Id);
                reviewRepository.DeleteReview(review);
                await reviewRepository.SaveReviewAsync();
            }

            personGameRepository.DeletePersonGame(personGame);
            await personGameRepository.SavePersonGameAsync();

            return Ok("Successfully deleted");
        }
    }
}
