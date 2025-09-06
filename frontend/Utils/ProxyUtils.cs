using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace frontend.Utils;

public static class ProxyUtils
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string GetCurrentMethod()
    {
        var st = new StackTrace();
        var sf = st.GetFrame(1);
        if (sf == null)
        {
            throw new Exception("GetCurrentMethod failed - StackFrame is null.");
        }
        var mth = sf.GetMethod();
        if (mth == null)
        {
            throw new Exception("GetCurrentMethod failed - Method is null.");
        }

        return mth.Name;
    }
}