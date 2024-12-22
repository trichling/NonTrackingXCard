namespace NonTrackingCustomerCard.Client.Contracts;

public class VendorOfCustomerData
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PublicKey { get; set; }
}

public class VendorData : VendorOfCustomerData
{

    public string PrivateKey { get; set; }

}

public class KeyPair
{
    public string PublicKey { get; set; }
    public string PrivateKey { get; set; }
}

public class CustomerData
{
    public Guid Id { get; set; }
    public int Points { get; set; }
}

public class CustomerDataWithSignature
{
    public CustomerDataWithSignature()
    {
        Customer = new();
        Signature = string.Empty;
    }

    public CustomerData Customer { get; set; }

    public string Signature { get; set; }
}

public class CustomerOfVendorData : CustomerDataWithSignature
{

    public CustomerOfVendorData() : base()
    {
        OfVendor = new();
    }

    public VendorOfCustomerData OfVendor { get; set; }

}



public class CreateCustomerData
{
    public CustomerData Customer { get; set; }
    public VendorOfCustomerData OfVendor { get; set; }
    public string LastValidCustomerData { get; set; }
    public string LastValidCutomerDataSignature { get; set; }
}

public enum ValidityState
{
    NoSignature,
    ValidSignature,
    InvalidSignature
}