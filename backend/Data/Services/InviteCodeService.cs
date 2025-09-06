using backend.Data.Context;
using backend.Data.Services.Interfaces;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Shared.Data.Entities;

namespace backend.Data.Services;

public class InviteCodeService : IInviteCodeService
{
    
    private readonly DataContext _context;
    private readonly ILogger<InviteCodeService> _logger;

    public InviteCodeService(DataContext context, ILogger<InviteCodeService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<InviteCode>> GenerateNewCode(User? user)
    {
        InviteCode code;
        if (user is not null)
        {
            code = new InviteCode(user);
        }
        else
        {
            code = new InviteCode();
        }

        await _context.InviteCodesDb.AddAsync(code);
        await _context.SaveChangesAsync();
        return code;
    }

    public async Task<Maybe<InviteCode>> FindCode(Guid code)
    {
        try
        {
            return await _context.InviteCodesDb.FirstAsync(c => c.Code == code);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Couldn't find invite code: {code}", code);
            return null;
        }
    }

    public async Task<Result<List<InviteCode>>> GetAllCodesByUser(User user)
    {
        return await _context.InviteCodesDb.Where(c => c.IssuerId != null && c.IssuerId.ToString() == user.Id).ToListAsync();
    }

    public async Task<Result<List<InviteCode>>> GetAllCodes()
    {
        return await _context.InviteCodesDb.ToListAsync();
    }

    public async Task<Result<List<InviteCode>>> GetAllCodesByStatus(bool isUsed = false)
    {
        return await _context.InviteCodesDb.Where(c => c.IsUsed == isUsed).ToListAsync();
    }

    public async Task<Result> SetCodeStatus(Guid code, bool isUsed = true)
    {
        try
        {
            var inviteCode = await _context.InviteCodesDb.FirstAsync(c => c.Code == code);
            inviteCode.IsUsed = isUsed;
            _context.Update(inviteCode);
            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Couldn't find invite code: {code}", code);
            return Result.Failure($"Couldn't find invite code: {code}\n{e}");
        }
    }

    public async Task<Result> DeleteCode(Guid code)
    {
        try
        {
            var inviteCode = await _context.InviteCodesDb.FirstAsync(c => c.Code == code);
            _context.Remove(inviteCode);
            return Result.Success();
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Couldn't find invite code: {code}", code);
            return Result.Failure($"Couldn't find invite code: {code}\n{e}");
        }
    }

    public async Task<Result> DeleteCodesByUser(User user)
    {
        var inviteCodes = await _context.InviteCodesDb.Where(c => c.IssuerId != null && c.IssuerId.ToString() == user.Id).ToListAsync();
        _context.RemoveRange(inviteCodes);
        return Result.Success();
    }

    public async Task<Result> ValidateCode(Guid inviteCode)
    {
        try
        {
            var code = await _context.InviteCodesDb.FirstAsync(c => c.Code == inviteCode && c.IsUsed == false);
            return Result.Success();
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Couldn't validate invite code: {code}", inviteCode);
            return Result.Failure($"Couldn't validate invite code: {inviteCode}\n{e}");
        }
    }
}