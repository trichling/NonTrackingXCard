using System.Text.Json.Serialization;

namespace NonTrackingCustomerCard.Client.Contracts;

public class VendorPublicData
{
    [JsonPropertyName("vn")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("puk")]
    public string PublicKey { get; set; } = string.Empty;

    [JsonPropertyName("svn")]
    public string SignedVendorName { get; set; } = string.Empty;
}

public class VendorData : VendorPublicData
{

    [JsonPropertyName("prk")]
    public string PrivateKey { get; set; } = string.Empty;



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

    [JsonPropertyName("t")]
    public long Timestamp { get; set; }
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
    public VendorPublicData OfVendor { get; set; }

}

public enum ValidityState
{
    NoSignature,
    ValidSignature,
    InvalidSignature
}