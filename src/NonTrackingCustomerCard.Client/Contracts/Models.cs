using System.Text.Json.Serialization;

namespace NonTrackingCustomerCard.Client.Contracts;

public class VendorOfCustomerData
{
    [JsonPropertyName("vn")]
    public string Name { get; set; }
    [JsonPropertyName("puk")]
    public string PublicKey { get; set; }
}

public class VendorData : VendorOfCustomerData
{

    [JsonPropertyName("prk")]
    public string PrivateKey { get; set; }

}

public class KeyPair
{
    public string PublicKey { get; set; }
    public string PrivateKey { get; set; }
}

public class CustomerData
{
    [JsonPropertyName("pts")]
    public int Points { get; set; }
}

public class CustomerDataWithSignature
{
    public CustomerDataWithSignature()
    {
        Customer = new();
        Signature = string.Empty;
    }

    [JsonPropertyName("c")]
    public CustomerData Customer { get; set; }

    [JsonPropertyName("s")]
    public string Signature { get; set; }
}

public class CustomerOfVendorData : CustomerDataWithSignature
{

    public CustomerOfVendorData() : base()
    {
        OfVendor = new();
    }

    [JsonPropertyName("ov")]
    public VendorOfCustomerData OfVendor { get; set; }

}

public enum ValidityState
{
    NoSignature,
    ValidSignature,
    InvalidSignature
}