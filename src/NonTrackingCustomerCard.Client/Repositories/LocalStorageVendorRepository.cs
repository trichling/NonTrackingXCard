
using System.Diagnostics.CodeAnalysis;

using Microsoft.JSInterop;

using NonTrackingCustomerCard.Client.Contracts;

namespace NonTrackingCustomerCard.Client.Repositories;

public class LocalStorageVendorRepository : IVendorRepository
{
    private readonly IJSRuntime _jSRuntime;

    public LocalStorageVendorRepository(IJSRuntime jSRuntime)
    {
        _jSRuntime = jSRuntime;
    }



    public async Task<VendorData> GetVendorData()
    {
        var (hasData, vendorJson) = await TryGetVendorJson();
        if (!hasData)
        {
            return new VendorData();
        }

        return System.Text.Json.JsonSerializer.Deserialize<VendorData>(vendorJson);
    }

    public async Task<VendorPublicData> GetVendorPublicData()
    {
        var (hasData, vendorJson) = await TryGetVendorJson();
        if (!hasData)
        {
            return new VendorPublicData();
        }

        return System.Text.Json.JsonSerializer.Deserialize<VendorPublicData>(vendorJson);
    }

    public async Task SaveVendorData(VendorData vendor)
    {
        var vendorJson = System.Text.Json.JsonSerializer.Serialize(vendor);
        await _jSRuntime.InvokeVoidAsync("localStorage.setItem", "NonTrackingCustomerCard.Client.Vendor", vendorJson);
    }

    public async Task DeleteVendorData()
    {
        await _jSRuntime.InvokeVoidAsync("localStorage.removeItem", "NonTrackingCustomerCard.Client.Vendor");
    }

    public async Task<(bool HasData, string VendorJson)> TryGetVendorJson()
    {
        var vendorJson = await _jSRuntime.InvokeAsync<string>("localStorage.getItem", "NonTrackingCustomerCard.Client.Vendor");
        if (string.IsNullOrEmpty(vendorJson))
        {
            return (false, string.Empty);
        }

        return (true, vendorJson);

    }
}