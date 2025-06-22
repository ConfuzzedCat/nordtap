namespace frontend.Web;

public class InternalApiResult<T>
{
    public InternalApiResult(T? value, bool success, params List<string> errors)
    {
        Succeeded = success;
        Data = value;
        Failed = !Succeeded;
        Errors = errors;
    }
    
    public InternalApiResult(T? value, params List<string> errors)
    {
        Succeeded = value != null;
        Data = value;
        Failed = !Succeeded;
        Errors = errors;
    }
    public bool Succeeded { get; private set; }
    public bool Failed { get; }

    private List<string> Errors { get; set; }
    private T? Data { get; set; }

    public T GetResult()
    {
        if (Succeeded && Data != null)
        {
            return Data;
        }
        throw new InvalidOperationException("Result was not successful or null.");
    }

    public string GetErrorMessage()
    {
        return Succeeded ? string.Empty : string.Join(Environment.NewLine, Errors.ToArray());
    }
}