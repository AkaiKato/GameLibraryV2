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
        public async Task<IActionResult> GetDLCs()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var DLC = mapper.Map<List<GameSmallListDto>>(await gameRepository.GetDLCsAsync());

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
        public async Task<IActionResult> GetDLCById(int dlcId)
        {
            if(!await gameRepository.DLCExistsAsync(dlcId))
                return NotFound($"Not found dlc with such id {dlcId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var DLC = mapper.Map<GameDto>(await gameRepository.GetDLCByIdAsync(dlcId));

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
        public async Task<IActionResult> CreateGameDlc([FromBody] DlcUpdate addDlc)
        {
            if (addDlc == null)
                return BadRequest(ModelState);

            if (!await gameRepository.GameExistsAsync(addDlc.ParentGameId))
                return NotFound($"Not found game with such id {addDlc.ParentGameId}");

            if (!await gameRepository.DLCExistsAsync(addDlc.DLCGameId))
                return NotFound($"Not found dlc with such id {addDlc.ParentGameId}");

            if (await dlcRepository.DLCExistsAsync(addDlc.ParentGameId, addDlc.DLCGameId))
                return BadRequest("DLC already have parent Game");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var dlcGame = await gameRepository.GetDLCByIdAsync(addDlc.DLCGameId);
            var parGame = await gameRepository.GetGameByIdAsync(addDlc.ParentGameId);
            var dlc = new DLC() { ParentGame = parGame, DLCGame = dlcGame };

            dlcGame.ParentGame = parGame;

            dlcRepository.DLCCreate(dlc);
            await dlcRepository.SaveDLCAsync();

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
        public async Task<IActionResult> DeleteGameDlc([FromBody] DlcUpdate deleteDlc)
        {
            if (deleteDlc == null)
                return BadRequest(ModelState);

            if (!await gameRepository.GameExistsAsync(deleteDlc.ParentGameId))
                return NotFound($"Not found game with such id {deleteDlc.ParentGameId}");

            if (!await gameRepository.DLCExistsAsync(deleteDlc.DLCGameId))
                return NotFound($"Not found dlc with such id {deleteDlc.DLCGameId}");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var dlc = await dlcRepository.GetDLCConnByIdAsync(deleteDlc.ParentGameId, deleteDlc.DLCGameId);

            dlc.DLCGame.ParentGame = null;

            dlcRepository.DLCDelete(dlc);
            await dlcRepository.SaveDLCAsync();

            return Ok("Successfully deleted");
        }

    }
}
