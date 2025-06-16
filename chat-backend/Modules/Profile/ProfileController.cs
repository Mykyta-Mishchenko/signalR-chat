using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace chat_backend.Modules.Profile.Controllers
{
    [Route("api/profile")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> SetUserProfileImg(UserProfileDTO userProfile)
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (ModelState.IsValid)
            {
                await _profileService.SetUserProfileByRefreshTokenAsync(refreshToken, userProfile.ProfileImg);
            }
            return Ok();
        }

        [HttpGet("image/{userId:int}")]
        public async Task<IActionResult> GetProfileImage(int userId)
        {
            var imageBytes = await _profileService.GetUserProfileAsync(userId);
            if(imageBytes == null)
            {
                return NotFound();
            }
            return File(imageBytes, "image/jpeg");
        }
    }
}
