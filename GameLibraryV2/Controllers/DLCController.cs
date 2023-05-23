using AutoMapper;
using GameLibraryV2.Dto.Common;
using GameLibraryV2.Dto.smallInfo;
using GameLibraryV2.Dto.Update;
using GameLibraryV2.Interfaces;
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
        /// <param name="dlcConnectionId"></param>
        /// <returns></returns>
        [HttpDelete("deleteDlc")]
        public async Task<IActionResult> DeleteGameDlc([FromQuery] int dlcConnectionId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!await dlcRepository.DLCExistsByConnIdAsync(dlcConnectionId))
                return NotFound("Not found such connection");

            var dlc = await dlcRepository.GetDLCConnByIdAsync(dlcConnectionId);

            dlc.DLCGame.ParentGame = null;

            dlcRepository.DLCDelete(dlc);
            await dlcRepository.SaveDLCAsync();

            return Ok("Successfully deleted");
        }

    }
}
