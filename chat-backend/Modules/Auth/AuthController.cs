using AutoMapper;
using chat_backend.Modules.Auth.DTO;
using chat_backend.Modules.Auth.Interfaces.Services;
using chat_backend.Modules.Auth.Models;
using chat_backend.Shared.Data.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace chat_backend.Modules.Auth
{
    [Route("api/auth")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthorizationController(
            IMapper mapper,
            IAuthService authService,
            ITokenService tokenService
            )
        {
            _mapper = mapper;
            _authService = authService;
            _tokenService = tokenService;
        }
        [HttpPost("signIn")]
        public async Task<IActionResult> SignIn(SignInDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Check your sign in form.");
            }
            var user = _mapper.Map<User>(request);
            var result = await _authService.SignInAsync(user);

            if (!result.Succeeded)
            {
                return Unauthorized("Invalid email or password.");
            }

            _tokenService.AppendRefreshToken(HttpContext, result.Tokens.RefreshToken);

            return Ok(new { result.Tokens.AccessToken });

        }

        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp(SignUpDTO request)
        {
            if (!ModelState.IsValid || request.Password != request.ConfirmedPassword)
            {
                return BadRequest("Check your sign up form.");
            }

            var user = _mapper.Map<User>(request);
            var result = await _authService.SignUpAsync(user, UserRole.user);
            if (!result.Succeeded)
            {
                return BadRequest("Can't sign up user. Try again.");
            }
            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (refreshToken == null)
            {
                return Unauthorized();
            }

            var result = await _authService.RefreshSessionAsync(refreshToken);
            if (result != null)
            {
                _tokenService.AppendRefreshToken(HttpContext, result.RefreshToken);
                return Ok(new { result.AccessToken });
            }

            return Unauthorized();
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            _authService.Logout(refreshToken ?? "");
            return Ok();
        }
    }
}
