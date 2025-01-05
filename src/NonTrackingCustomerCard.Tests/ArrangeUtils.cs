using Microsoft.JSInterop;

using NSubstitute;

namespace NonTrackingCustomerCard.Tests;

public static class ArrangeUtils 
{

    public static void ArrangeLocalStorageGetItemForKeyReturns(this IJSRuntime mockJSRuntime, string key, string value)
    {
        mockJSRuntime
            .InvokeAsync<string>(
                "localStorage.getItem",
                Arg.Is<object[]>(args =>
                    args.Length == 1
                    && args[0].ToString() == key
                )
            )
            .Returns(ValueTask.FromResult(value));
    }

}