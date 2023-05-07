using GameLibraryV2.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : Controller
    {
        private readonly IDeveloperRepository developerRepository;
        private readonly IGameRepository gameRepository;
        private readonly IPublisherRepository publisherRepository;
        private readonly IUserRepository userRepository;

        public PictureController(IDeveloperRepository _developerRepository,
            IGameRepository _gameRepository,
            IPublisherRepository _publisherRepository,
            IUserRepository _userRepository)
        {
            developerRepository = _developerRepository;
            gameRepository = _gameRepository;
            publisherRepository = _publisherRepository;
            userRepository = _userRepository;
        }

        private readonly string[] permittedExtensions = { ".jpg", ".jpeg", ".png" };

        /// <summary>
        /// Update developer picture
        /// </summary>
        /// <param name="developerId"></param>
        /// <param name="pic"></param>
        /// <returns></returns>
        [HttpPut("uploadDeveloperPicture")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UploadDeveloperPicture([FromQuery] int developerId, IFormFile pic)
        {
            if (pic == null)
                return BadRequest(ModelState);

            var ext = Path.GetExtension(pic.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return BadRequest("Unsupported extension");

            if (!await developerRepository.DeveloperExistsAsync(developerId))
                return NotFound($"Not found developer with such id {developerId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var unique = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var developer = await developerRepository.GetDeveloperByIdAsync(developerId);
            var newfilePath = $"\\Images\\developerPicture\\{unique}{ext}";
            var oldfilePath = developer.PicturePath;

            using var stream = new FileStream(newfilePath, FileMode.Create);
            pic.CopyTo(stream);

            developer!.PicturePath = newfilePath;

            developerRepository.UpdateDeveloper(developer);
            await developerRepository.SaveDeveloperAsync();

            if (oldfilePath.Trim() != $"\\Images\\developerPicture\\Def.jpg")
            {
                FileInfo f = new(oldfilePath);
                f.Delete();
            }

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Update developer mini picture
        /// </summary>
        /// <param name="developerId"></param>
        /// <param name="pic"></param>
        /// <returns></returns>
        [HttpPut("uploadDeveloperMiniPicture")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UploadDeveloperMiniPicture([FromQuery] int developerId, IFormFile pic)
        {
            if (pic == null)
                return BadRequest(ModelState);

            var ext = Path.GetExtension(pic.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return BadRequest("Unsupported extension");

            if (!await developerRepository.DeveloperExistsAsync(developerId))
                return NotFound($"Not found developer with such id {developerId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var unique = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var developer = await developerRepository.GetDeveloperByIdAsync(developerId);
            var newfilePath = $"\\Images\\developerMiniPicture\\{unique}{ext}";
            var oldfilePath = developer.MiniPicturePath;

            using var stream = new FileStream(newfilePath, FileMode.Create);
            pic.CopyTo(stream);

            developer!.MiniPicturePath = newfilePath;

            developerRepository.UpdateDeveloper(developer);
            await developerRepository.SaveDeveloperAsync();

            if (oldfilePath.Trim() != $"\\Images\\developerMiniPicture\\Def.jpg")
            {
                FileInfo f = new(oldfilePath);
                f.Delete();
            }

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Update Game Picture
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="pic"></param>
        /// <returns></returns>
        [HttpPut("uploadGamePicture")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UploadGamePicture([FromQuery] int gameId, IFormFile pic)
        {
            if (pic == null)
                return BadRequest(ModelState);

            var ext = Path.GetExtension(pic.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return BadRequest("Unsupported extension");

            if (!await gameRepository.GameExistsAsync(gameId))
                return NotFound($"Not found game with such id {gameId}");

            var unique = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var game = await gameRepository.GetGameByIdAsync(gameId);
            var newfilePath = $"\\Images\\gamePicture\\{unique}{ext}";
            var oldfilePath = game.PicturePath;

            using var stream = new FileStream(newfilePath, FileMode.Create);
            pic.CopyTo(stream);

            game!.PicturePath = newfilePath;

            gameRepository.UpdateGame(game);
            await gameRepository.SaveGameAsync();

            if (oldfilePath.Trim() != $"\\Images\\gamePicture\\Def.jpg")
            {
                FileInfo f = new(oldfilePath);
                f.Delete();
            }

            return Ok("Successfully updated");
        }

        /// <summary>
        /// Update publisher picture
        /// </summary>
        /// <param name="publisherId"></param>
        /// <param name="pic"></param>
        /// <returns></returns>
        [HttpPut("uploadPublisherPicture")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UploadPublisherPicture([FromQuery] int publisherId, IFormFile pic)
        {
            if (pic == null)
                return BadRequest(ModelState);

            var ext = Path.GetExtension(pic.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return BadRequest("Unsupported extension");

            if (!await publisherRepository.PublisherExistsAsync(publisherId))
                return NotFound($"Not found publisher with such id {publisherId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var unique = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var publisher = await publisherRepository.GetPublisherByIdAsync(publisherId);
            var newfilePath = $"\\Images\\publisherPicture\\{unique}{ext}";
            var oldfilePath = publisher.PicturePath;

            using var stream = new FileStream(newfilePath, FileMode.Create);
            pic.CopyTo(stream);
            publisher!.PicturePath = newfilePath;

            publisherRepository.UpdatePublisher(publisher);
            await publisherRepository.SavePublisherAsync();

            if (oldfilePath.Trim() != $"\\Images\\publisherPicture\\Def.jpg")
            {
                FileInfo f = new(oldfilePath);
                f.Delete();
            }


            return Ok("Successfully updated");
        }

        /// <summary>
        /// Update publisher mini picture
        /// </summary>
        /// <param name="publisherId"></param>
        /// <param name="pic"></param>
        /// <returns></returns>
        [HttpPut("uploadPublisherMiniPicture")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UploadPublihserMiniPicture([FromQuery] int publisherId, IFormFile pic)
        {
            if (pic == null)
                return BadRequest(ModelState);

            var ext = Path.GetExtension(pic.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return BadRequest("Unsupported extension");

            if (!await publisherRepository.PublisherExistsAsync(publisherId))
                return NotFound($"Not found publisher with such id {publisherId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var unique = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var publisher = await publisherRepository.GetPublisherByIdAsync(publisherId);
            var newfilePath = $"\\Images\\publisherMiniPicture\\{unique}{ext}";
            var oldfilePath = publisher.MiniPicturePath;

            using var stream = new FileStream(newfilePath, FileMode.Create);
            pic.CopyTo(stream);
            publisher!.MiniPicturePath = newfilePath;

            publisherRepository.UpdatePublisher(publisher);
            await publisherRepository.SavePublisherAsync();

            if (oldfilePath.Trim() != $"\\Images\\publisherMiniPicture\\Def.jpg")
            {
                FileInfo f = new(oldfilePath);
                f.Delete();
            }

            return Ok("Successfully updated");
        }


        /// <summary>
        /// Update User Picture
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pic"></param>
        /// <returns></returns>
        [HttpPut("uploadUserPicture")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UploadUserPicture([FromQuery] int userId, IFormFile pic)
        {
            if (pic == null)
                return BadRequest(ModelState);

            var ext = Path.GetExtension(pic.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return BadRequest("Unsupported extension");

            if (!await userRepository.UserExistsByIdAsync(userId))
                return NotFound($"Not found user with such id {userId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var unique = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var user = await userRepository.GetUserByIdAsync(userId);
            var newfilePath = $"\\Images\\userPicture\\{unique}{ext}";
            var oldfilePath = user.PicturePath;

            using var stream = new FileStream(newfilePath, FileMode.Create);
            pic.CopyTo(stream);

            user!.PicturePath = newfilePath;

            userRepository.UpdateUser(user);
            await userRepository.SaveUserAsync();

            if (oldfilePath.Trim() != $"\\Images\\userPicture\\Def.jpg")
            {
                FileInfo f = new(oldfilePath);
                f.Delete();
            }

            return Ok("Successfully updated");
        }


        protected internal static string PathToUrl(string path)
        {
            return path.Replace("\\","/");
        }
    }
}
