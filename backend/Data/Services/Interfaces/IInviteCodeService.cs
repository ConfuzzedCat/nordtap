using CSharpFunctionalExtensions;
using Shared.Data.Entities;

namespace backend.Data.Services.Interfaces;

public interface IInviteCodeService
{
    Task<Result<InviteCode>> GenerateNewCode(User? user);
    Task<Maybe<InviteCode>> FindCode(Guid code);
    Task<Result<List<InviteCode>>> GetAllCodesByUser(User user);
    Task<Result<List<InviteCode>>> GetAllCodes();
    Task<Result<List<InviteCode>>> GetAllCodesByStatus(bool isUsed = false);
    Task<Result> SetCodeStatus(Guid code, bool isUsed = true);
    Task<Result> DeleteCode(Guid code);
    Task<Result> DeleteCodesByUser(User user);
    Task<Result> ValidateCode(Guid inviteCode);
}