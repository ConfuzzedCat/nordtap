using System.Security.Claims;
using backend.Data.Entities;
using backend.Extensions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TwoFactorAuthController : ControllerBase
    {
        private readonly ILogger<TwoFactorAuthController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TwoFactorAuthController(IHttpContextAccessor httpContextAccessor, ILogger<TwoFactorAuthController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet(Name = "GetAuthKey")]
        public async Task<IActionResult> GetTwoFactorAuthUnformattedKey()
        {
            bool firstTimeEnabled = false;
            var claims = _httpContextAccessor.HttpContext?.User;
            if (claims is null)
            {
                _logger.LogWarning("GetTwoFactorAuthUnformattedKey: claims was null");
                return null;
                //return await _codeService.GenerateNewCode(null);
            }
            var user = await _userManager.GetUserAsync(claims);

            if (user is null)
            {
                _logger.LogWarning("GetTwoFactorAuthUnformattedKey: user was null");
                return Forbid();
            }
        
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
                firstTimeEnabled = true;
            }
            return Ok( new TwoFactorAuthResponse
            {
                UnformattedKey = unformattedKey,
                Username = user.UserName,
                FirstTimeEnabled = firstTimeEnabled,
                RecoveryCodes = []
            });
        }
    }
}