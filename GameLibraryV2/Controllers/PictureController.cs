using GameLibraryV2.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;

namespace GameLibraryV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
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

        private readonly string[] permittedExtensions = { ".jpg", ".jpeg", ".webp" };

        /// <summary>
        /// Update developer picture
        /// </summary>
        /// <param name="developerId"></param>
        /// <param name="pic"></param>
        /// <returns></returns>
        [HttpPut("uploadDeveloperPicture")]
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
            var newfilePath = $"/uploads/developerPicture/{unique}{ext}";
            var oldfilePath = developer.PicturePath;

            var t = AppContext.BaseDirectory;
            var tt = Directory.GetParent(t);
            var ttt = Directory.GetParent(tt!.FullName);

            using (FileStream fileStream = System.IO.File.Create(ttt!.FullName + newfilePath))
            pic.CopyTo(fileStream);

            developer!.PicturePath = newfilePath;

            developerRepository.UpdateDeveloper(developer);
            await developerRepository.SaveDeveloperAsync();

            if (oldfilePath.Trim() != $"/uploads/developerPicture/Def.jpg")
            {
                FileInfo f = new(ttt.FullName + oldfilePath);
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
            var newfilePath = $"/uploads/developerMiniPicture/{unique}{ext}";
            var oldfilePath = developer.MiniPicturePath;

            var t = AppContext.BaseDirectory;
            var tt = Directory.GetParent(t);
            var ttt = Directory.GetParent(tt!.FullName);

            using (FileStream fileStream = System.IO.File.Create(ttt!.FullName + newfilePath))
            pic.CopyTo(fileStream);

            developer!.MiniPicturePath = newfilePath;

            developerRepository.UpdateDeveloper(developer);
            await developerRepository.SaveDeveloperAsync();

            if (oldfilePath.Trim() != $"/uploads/developerMiniPicture/Def.jpg")
            {
                FileInfo f = new(ttt!.FullName + oldfilePath);
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
            var newfilePath = $"/uploads/gamePicture/{unique}{ext}";
            var oldfilePath = game.PicturePath;

            var t = AppContext.BaseDirectory;
            var tt = Directory.GetParent(t);
            var ttt = Directory.GetParent(tt!.FullName);

            using (FileStream fileStream = System.IO.File.Create(ttt!.FullName + newfilePath))
            pic.CopyTo(fileStream);

            game!.PicturePath = newfilePath;

            gameRepository.UpdateGame(game);
            await gameRepository.SaveGameAsync();

            if (oldfilePath.Trim() != $"/uploads/gamePicture/Def.jpg")
            {
                FileInfo f = new(ttt!.FullName + oldfilePath);
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
            var newfilePath = $"/uploads/publisherPicture/{unique}{ext}";
            var oldfilePath = publisher.PicturePath;

            var t = AppContext.BaseDirectory;
            var tt = Directory.GetParent(t);
            var ttt = Directory.GetParent(tt!.FullName);

            using (FileStream fileStream = System.IO.File.Create(ttt!.FullName + newfilePath))
            pic.CopyTo(fileStream);

            publisher!.PicturePath = newfilePath;

            publisherRepository.UpdatePublisher(publisher);
            await publisherRepository.SavePublisherAsync();

            if (oldfilePath.Trim() != $"/uploads/publisherPicture/Def.jpg")
            {
                FileInfo f = new(ttt!.FullName + oldfilePath);
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
            var newfilePath = $"/uploads/publisherMiniPicture/{unique}{ext}";
            var oldfilePath = publisher.MiniPicturePath;

            var t = AppContext.BaseDirectory;
            var tt = Directory.GetParent(t);
            var ttt = Directory.GetParent(tt!.FullName);

            using (FileStream fileStream = System.IO.File.Create(ttt!.FullName + newfilePath))
            pic.CopyTo(fileStream);

            publisher!.MiniPicturePath = newfilePath;

            publisherRepository.UpdatePublisher(publisher);
            await publisherRepository.SavePublisherAsync();

            if (oldfilePath.Trim() != $"/uploads/publisherMiniPicture/Def.jpg")
            {
                FileInfo f = new(ttt!.FullName + oldfilePath);
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
        [Authorize(Roles = "user")]
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
            var newfilePath = $"/uploads/userPicture/{unique}{ext}";
            var oldfilePath = user.PicturePath;

            var t = AppContext.BaseDirectory;
            var tt = Directory.GetParent(t);
            var ttt = Directory.GetParent(tt!.FullName);

            using (FileStream fileStream = System.IO.File.Create(ttt!.FullName + newfilePath))
            pic.CopyTo(fileStream);

            user!.PicturePath = newfilePath;

            userRepository.UpdateUser(user);
            await userRepository.SaveUserAsync();

            if (oldfilePath.Trim() != $"/uploads/userPicture/Def.jpg")
            {
                FileInfo f = new(ttt!.FullName + oldfilePath);
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
