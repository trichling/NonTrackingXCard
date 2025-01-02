using NonTrackingCustomerCard.Client.Contracts;

namespace NonTrackingCustomerCard.Client.Repositories;

public interface IVendorRepository
{
    Task<(bool HasData, string VendorJson)> TryGetVendorJson();

    Task<VendorData> GetVendorData();

    Task<VendorOfCustomerData> GetVendorPublicData();

    Task SaveVendorData(VendorData vendorData);
}