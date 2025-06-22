using System.Security.Claims;

namespace frontend.Web;

public class LoginResult
{
    public InternalApiResult<Cookie> Result { get; }
    public Status ResultStatus { get; private set; } = Status.Succeeded;
    public ClaimsIdentity ClaimsIdentity { get; private set; } = new ClaimsIdentity();
    public Cookie Cookie => Result.GetResult();

    public LoginResult(InternalApiResult<Cookie> result)
    {
        Result = result;
        if (result.Succeeded)
        {
            ResultStatus = Status.Succeeded;
            return;
        }
        ResultStatus = Status.Failed;
    }

    public LoginResult(InternalApiResult<Cookie> result, string failReason)
    {
        Result = result;

        switch (failReason)
        {
            case "RequiresTwoFactor":
                ResultStatus = Status.RequiresTwoFactor;
                break;
            case "NotAllowed":
                ResultStatus = Status.NotAllowed;
                break;
            case "LockedOut":
                ResultStatus = Status.LockedOut;
                break;
            default:
                if (result.Succeeded)
                {
                    ResultStatus = Status.Succeeded;
                    break;
                }
                ResultStatus = Status.Failed;
                break;
        }
    }

    public enum Status
    {
        Succeeded,
        Failed,
        LockedOut,
        NotAllowed,
        RequiresTwoFactor
    }
}