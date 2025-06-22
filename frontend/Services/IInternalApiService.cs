using backend.Data.Entities;
using backend.Extensions.Models;
using frontend.Web;
using Microsoft.AspNetCore.Identity.Data;
using InfoRequest = backend.Extensions.Models.InfoRequest;
using InfoResponse = backend.Extensions.Models.InfoResponse;
using LoginRequest = backend.Extensions.Models.LoginRequest;
using RegisterRequest = backend.Extensions.Models.RegisterRequest;

namespace frontend.Services;

public interface IInternalApiService
{
    #region UserAuth
    Task<string> GetUsername(string cookie);
    Task<string[]> GetRoles(string cookie);
    Task<InfoResponse?> GetUserInfo(string cookie);
    Task<InfoResponse?> PostUserInfo(string cookie, InfoRequest request);
    Task<LoginResult> PostLogin(LoginRequest request);
    Task<TwoFactorResponse?> PostTwoFactorAuth(string cookie, TwoFactorRequest request);
    Task<bool> PostRegister(RegisterRequest request);
        #region TwoFactorAuth
        Task<TwoFactorAuthResponse?> GetTwoFactorAuthUnformattedKey(string cookie);
        #endregion
    #endregion
    
    #region InviteCode
    Task<InviteCode?> GetInviteCode(string cookie);
    #endregion

}