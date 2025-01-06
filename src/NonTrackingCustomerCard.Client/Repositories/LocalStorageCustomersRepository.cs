using Microsoft.JSInterop;

using NonTrackingCustomerCard.Client.Contracts;

namespace NonTrackingCustomerCard.Client.Repositories;

public class LocalStorageCustomersRepository : ICustomersRepository
{
    private readonly IJSRuntime _jSRuntime;

    public LocalStorageCustomersRepository(IJSRuntime jSRuntime)
    {
        _jSRuntime = jSRuntime;
    }


    public async Task<CustomerDataWithSignature> GetCustomerDataWithSignatureAsync()
    {
        var (hasData, customerJson) = await TryGetCustomerJson();

        if (!hasData)
        {
            return new CustomerDataWithSignature();
        }

        var customerOfVendor = System.Text.Json.JsonSerializer.Deserialize<CustomerDataWithSignature>(customerJson) ?? new CustomerOfVendorData();

        return customerOfVendor;
    }

    public async Task<CustomerOfVendorData> GetCustomerOfVendorDataAsync()
    {
        var (hasData, customerJson) = await TryGetCustomerJson();

        if (!hasData)
        {
            return new CustomerOfVendorData();
        }

        var customerOfVendor = System.Text.Json.JsonSerializer.Deserialize<CustomerOfVendorData>(customerJson) ?? new CustomerOfVendorData();

        return customerOfVendor;
    }

    public async Task SaveCustomerDataWithSignatureAsync(CustomerOfVendorData customerData)
    {
        var customerJson = System.Text.Json.JsonSerializer.Serialize(customerData);
        await _jSRuntime.InvokeVoidAsync("localStorage.setItem", "NonTrackingCustomerCard.Client.Customer", customerJson);
    }


    public async Task DeleteCustomerData()
    {
        await _jSRuntime.InvokeVoidAsync("localStorage.removeItem", "NonTrackingCustomerCard.Client.Customer");

    }

    public async Task<(bool HasData, string CustomerJson)> TryGetCustomerJson()
    {
        var customerJson = await _jSRuntime.InvokeAsync<string>("localStorage.getItem", "NonTrackingCustomerCard.Client.Customer");
        if (string.IsNullOrEmpty(customerJson))
        {
            return (false, string.Empty);
        }

        return (true, customerJson);
    }
}
