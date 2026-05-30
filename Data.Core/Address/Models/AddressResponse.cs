namespace Data.Core.Address.Models
{
    public class AddressResponse
    {
        public string Original { get; set; } = string.Empty;
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
    }
}
