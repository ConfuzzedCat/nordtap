using frontend.Web;
using Shared.Data.Entities;
using Shared.Extensions.Models;
using InviteCode = Shared.Data.DTO.InviteCode;

namespace frontend.Services;

public interface IInternalApiService
{
    #region UserAuth
    Task<string> GetUsername();
    Task<string[]> GetRoles();
    Task<InfoResponse?> GetUserInfo();
    Task<InfoResponse?> PostUserInfo(InfoRequest request);
    Task PostLogin(LoginRequest request);
    Task<bool> PostRegister(RegisterRequest request);
    #endregion
    
    #region InviteCode
    Task<InviteCode?> GetInviteCode(string cookie);
    #endregion
}