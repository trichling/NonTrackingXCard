using NonTrackingCustomerCard.Client.Contracts;

namespace NonTrackingCustomerCard.Client.Repositories;

public interface ICustomersRepository
{
    Task<(bool HasData, string CustomerJson)> TryGetCustomerJson();

    Task<CustomerOfVendorData> GetCustomerOfVendorDataAsync();

    Task<CustomerDataWithSignature> GetCustomerDataWithSignatureAsync();

    Task SaveCustomerDataWithSignatureAsync(CustomerOfVendorData customerData);

    Task DeleteCustomerData();
}