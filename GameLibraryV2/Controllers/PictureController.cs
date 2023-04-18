using GameLibraryV2.Interfaces;
using GameLibraryV2.Repositories;
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
        public IActionResult UploadDeveloperPicture([FromQuery] int developerId, IFormFile pic)
        {
            if (pic == null)
                return BadRequest(ModelState);

            var ext = Path.GetExtension(pic.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))

                return BadRequest("Unsupported extension");

            if (!developerRepository.DeveloperExists(developerId))
                return NotFound($"Not found developer with such id {developerId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var unique = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var developer = developerRepository.GetDeveloperById(developerId);
            var newfilePath = $"\\Images\\developerPicture\\{unique}{ext}";
            var oldfilePath = developer.PicturePath;

            using var stream = new FileStream(newfilePath, FileMode.Create);
            pic.CopyTo(stream);

            developer!.PicturePath = newfilePath;

            if (!developerRepository.UpdateDeveloper(developer))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

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
        public IActionResult UploadDeveloperMiniPicture([FromQuery] int developerId, IFormFile pic)
        {
            if (pic == null)
                return BadRequest(ModelState);

            var ext = Path.GetExtension(pic.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return BadRequest("Unsupported extension");

            if (!developerRepository.DeveloperExists(developerId))
                return NotFound($"Not found developer with such id {developerId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var unique = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var developer = developerRepository.GetDeveloperById(developerId);
            var newfilePath = $"\\Images\\developerMiniPicture\\{unique}{ext}";
            var oldfilePath = developer.MiniPicturePath;

            using var stream = new FileStream(newfilePath, FileMode.Create);
            pic.CopyTo(stream);

            developer!.MiniPicturePath = newfilePath;

            if (!developerRepository.UpdateDeveloper(developer))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

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
        public IActionResult UploadGamePicture([FromQuery] int gameId, IFormFile pic)
        {
            if (pic == null)
                return BadRequest(ModelState);

            var ext = Path.GetExtension(pic.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return BadRequest("Unsupported extension");

            if (!gameRepository.GameExists(gameId))
                return NotFound($"Not found game with such id {gameId}");

            var unique = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var game = gameRepository.GetGameById(gameId);
            var newfilePath = $"\\Images\\gamePicture\\{unique}{ext}";
            var oldfilePath = game.PicturePath;

            using var stream = new FileStream(newfilePath, FileMode.Create);
            pic.CopyTo(stream);

            game!.PicturePath = newfilePath;

            if (!gameRepository.UpdateGame(game))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

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
        public IActionResult UploadPublisherPicture([FromQuery] int publisherId, IFormFile pic)
        {
            if (pic == null)
                return BadRequest(ModelState);

            var ext = Path.GetExtension(pic.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return BadRequest("Unsupported extension");

            if (!publisherRepository.PublisherExists(publisherId))
                return NotFound($"Not found publisher with such id {publisherId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var unique = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var publisher = publisherRepository.GetPublisherById(publisherId);
            var newfilePath = $"\\Images\\publisherPicture\\{unique}{ext}";
            var oldfilePath = publisher.PicturePath;

            using var stream = new FileStream(newfilePath, FileMode.Create);
            pic.CopyTo(stream);
            publisher!.PicturePath = newfilePath;

            if (!publisherRepository.UpdatePublisher(publisher))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

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
        public IActionResult UploadPublihserMiniPicture([FromQuery] int publisherId, IFormFile pic)
        {
            if (pic == null)
                return BadRequest(ModelState);

            var ext = Path.GetExtension(pic.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return BadRequest("Unsupported extension");

            if (!publisherRepository.PublisherExists(publisherId))
                return NotFound($"Not found publisher with such id {publisherId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var unique = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var publisher = publisherRepository.GetPublisherById(publisherId);
            var newfilePath = $"\\Images\\publisherMiniPicture\\{unique}{ext}";
            var oldfilePath = publisher.MiniPicturePath;

            using var stream = new FileStream(newfilePath, FileMode.Create);
            pic.CopyTo(stream);
            publisher!.MiniPicturePath = newfilePath;

            if (!publisherRepository.UpdatePublisher(publisher))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

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
        public IActionResult UploadUserPicture([FromQuery] int userId, IFormFile pic)
        {
            if (pic == null)
                return BadRequest(ModelState);

            var ext = Path.GetExtension(pic.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                return BadRequest("Unsupported extension");

            if (!userRepository.UserExistsById(userId))
                return NotFound($"Not found user with such id {userId}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var unique = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var user = userRepository.GetUserById(userId);
            var newfilePath = $"\\Images\\userPicture\\{unique}{ext}";
            var oldfilePath = user.PicturePath;

            using var stream = new FileStream(newfilePath, FileMode.Create);
            pic.CopyTo(stream);

            user!.PicturePath = newfilePath;

            if (!userRepository.UpdateUser(user))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            if (oldfilePath.Trim() != $"\\Images\\userPicture\\Def.jpg")
            {
                FileInfo f = new(oldfilePath);
                f.Delete();
            }

            return Ok("Successfully updated");
        }
    }
}
