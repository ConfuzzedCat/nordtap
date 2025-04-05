using backend.Data.Entities;
using frontend.Web;
using backend.Extensions.Models;

namespace frontend.Services;

public interface IInternalApiService
{
    #region UserAuth
    Task<string> GetUsername(string cookie);
    Task<string[]> GetRoles(string cookie);
    Task<InfoResponse?> GetUserInfo(string cookie);
    Task<InfoResponse?> PostUserInfo(string cookie, InfoRequest request);
    Task<Cookie> PostLogin(LoginRequest request);
    Task<bool> PostRegister(RegisterRequest request);
    #endregion
    
    #region InviteCode
    Task<InviteCode?> GetInviteCode(string cookie);
    #endregion
}