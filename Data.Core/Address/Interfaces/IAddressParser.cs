using Data.Core.Address.Models;

namespace Data.Core.Address.Interfaces
{
    public interface IAddressParser
    {
        AddressResponse Parse(string address);
    }
}
