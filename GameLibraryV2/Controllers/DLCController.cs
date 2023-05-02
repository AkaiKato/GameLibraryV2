using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Interfaces;
using GameLibraryV2.Models;
using GameLibraryV2.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DLCController : Controller
    {
        private readonly IDLCRepository dlcRepository;
        private readonly IGameRepository gameRepository;
        private readonly IMapper mapper;

        public DLCController(IDLCRepository _dlcRepository, IGameRepository _gameRepository,IMapper _mapper)
        {
            dlcRepository = _dlcRepository;
            gameRepository = _gameRepository;
            mapper = _mapper;
        }

        /// <summary>
        /// Return all DLC
        /// </summary>
        /// <returns></returns>
        [HttpGet("dlcAll")]
        [ProducesResponseType(200, Type = typeof(List<GameSmallListDto>))]
        public IActionResult GetDLCs()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var DLC = mapper.Map<List<GameSmallListDto>>(gameRepository.GetDLCs());

            return Ok(DLC);
        }

        /// <summary>
        /// Return specified DLC
        /// </summary>
        /// <param name="dlcId"></param>
        /// <returns></returns>
        [HttpGet("{dlcId}")]
        [ProducesResponseType(200, Type = typeof(GameDto))]
        [ProducesResponseType(400)]
        public IActionResult GetDLCById(int dlcId)
        {
            if(!gameRepository.DLCExists(dlcId))
                return NotFound($"Not found dlc with such id {dlcId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var DLC = mapper.Map<GameDto>(gameRepository.GetDLCById(dlcId));

            return Ok(DLC);
        }

        /// <summary>
        /// Creates specified dlc connection
        /// </summary>
        /// <param name="addDlc"></param>
        /// <returns></returns>
        [HttpPost("createDlc")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateGameDlc([FromBody] DlcUpdate addDlc)
        {
            if (addDlc == null)
                return BadRequest(ModelState);

            if (!gameRepository.GameExists(addDlc.ParentGameId))
                return NotFound($"Not found game with such id {addDlc.ParentGameId}");

            if (!gameRepository.DLCExists(addDlc.DLCGameId))
                return NotFound($"Not found dlc with such id {addDlc.ParentGameId}");

            if (dlcRepository.DLCExists(addDlc.ParentGameId, addDlc.DLCGameId))
                return BadRequest("DLC already have parent Game");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var dlcGame = gameRepository.GetDLCById(addDlc.DLCGameId);
            var parGame = gameRepository.GetGameById(addDlc.ParentGameId);
            var dlc = new DLC() { ParentGame = parGame, DLCGame = dlcGame };

            dlcGame.ParentGame = parGame;

            if (!dlcRepository.DLCCreate(dlc))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        /// <summary>
        /// Deletes specified dlc connection
        /// </summary>
        /// <param name="deleteDlc"></param>
        /// <returns></returns>
        [HttpDelete("deleteDlc")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeleteGameDlc([FromBody] DlcUpdate deleteDlc)
        {
            if (deleteDlc == null)
                return BadRequest(ModelState);

            if (!gameRepository.GameExists(deleteDlc.ParentGameId))
                return NotFound($"Not found game with such id {deleteDlc.ParentGameId}");

            if (!gameRepository.DLCExists(deleteDlc.DLCGameId))
                return NotFound($"Not found dlc with such id {deleteDlc.DLCGameId}");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var dlc = dlcRepository.GetDLCConnById(deleteDlc.ParentGameId, deleteDlc.DLCGameId);

            dlc.DLCGame.ParentGame = null;

            if (!dlcRepository.DLCDelete(dlc))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }

    }
}
