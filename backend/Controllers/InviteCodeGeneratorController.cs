using System.Security.Claims;
using backend.Data.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Data.Entities;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "admin,trusted")]
public class InviteCodeGeneratorController : ControllerBase
{
    private readonly ILogger<InviteCodeGeneratorController> _logger;
    private readonly IInviteCodeService _codeService;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public InviteCodeGeneratorController(ILogger<InviteCodeGeneratorController> logger, IInviteCodeService codeService, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _codeService = codeService;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet(Name = "GetNewCode")]
    public async Task<InviteCode> Get()
    {
        var claims = _httpContextAccessor.HttpContext?.User;
        if (claims is null)
        {
            var nullUserResult = await _codeService.GenerateNewCode(null);
            if (nullUserResult.IsFailure)
            {
                throw new ApplicationException(nullUserResult.Error);
            }
            return nullUserResult.Value;
        }
        var user = await _userManager.GetUserAsync(claims);
        var result = await _codeService.GenerateNewCode(user);
        return result.Value;
    }
}