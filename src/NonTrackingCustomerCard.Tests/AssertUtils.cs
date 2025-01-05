namespace NonTrackingCustomerCard.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.JSInterop;

using NonTrackingCustomerCard.Client.Contracts;
using NonTrackingCustomerCard.Client.Repositories;

using NSubstitute;

public static class AssertUtils
{
    public static async Task AssertLocalStorageGetCallReceived(this IJSRuntime mockJSRuntime, string key)
    {
        await mockJSRuntime
                       .Received(1)
                       .InvokeAsync<string>(
                           "localStorage.getItem",
                           Arg.Is<object[]>(args =>
                               args.Length == 1
                               && args[0].ToString() == key
                           )
                       );
    }

    public static async Task AssertLocalStorageSetCallReceived(this IJSRuntime mockJSRuntime, string key, string value)
    {
        await mockJSRuntime
                .Received(1)
                .InvokeVoidAsync(
                    "localStorage.setItem",
                    Arg.Is<object[]>(args =>
                        args.Length == 2
                        && args[0].ToString() == key
                        && args[1].ToString() == value
                    )
                );
    }

    public static void AssertVendorDataEqual(VendorData expected, VendorData actual)
    {
        AssertVendorPublicDataEqual(expected, actual);
        Assert.Equal(expected.PrivateKey, actual.PrivateKey);
    }

    public static void AssertVendorPublicDataEqual(VendorPublicData expected, VendorPublicData actual)
    {
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.PublicKey, actual.PublicKey);
    }

    public static void AssertCustomerDataEqual(CustomerData expected, CustomerData actual)
    {
        Assert.Equal(expected.Points, actual.Points);
    }

    public static void AssertCustomerDataWithSignatureEqual(CustomerDataWithSignature expected, CustomerDataWithSignature actual)
    {
        AssertCustomerDataEqual(expected.Customer, actual.Customer);
        Assert.Equal(expected.Signature, actual.Signature);
    }

    public static void AssertCustomerOfVendorDataEqual(CustomerOfVendorData expected, CustomerOfVendorData actual)
    {
        AssertCustomerDataWithSignatureEqual(expected, actual);
        AssertVendorPublicDataEqual(expected.OfVendor, actual.OfVendor);
    }
}