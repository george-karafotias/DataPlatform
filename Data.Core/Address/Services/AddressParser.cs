using Data.Core.Address.Interfaces;
using Data.Core.Address.Models;
using System.Text.RegularExpressions;

namespace Data.Core.Address.Services
{
    public class AddressParser : IAddressParser
    {
        private static readonly Regex PostalCodeRegex = new(@"\b\d{5}\b");

        public AddressResponse Parse(string address)
        {
            var result = new AddressResponse
            {
                Original = address
            };

            if (string.IsNullOrWhiteSpace(address))
                return result;

            address = address.Trim();

            // -----------------------------------
            // STEP 1: Extract postal code first
            // -----------------------------------
            var postalMatch = PostalCodeRegex.Match(address);

            if (postalMatch.Success)
            {
                result.PostalCode = postalMatch.Value;

                // remove postal code from working string
                address = address.Replace(postalMatch.Value, "").Trim();
            }

            // -----------------------------------
            // STEP 2: Split by comma (if exists)
            // -----------------------------------
            var parts = address.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var streetPart = parts[0];

            if (parts.Length > 1)
            {
                var last = parts[^1];

                // only treat as city if it's not accidentally a number
                if (!Regex.IsMatch(last, @"^\d+$"))
                {
                    result.City = last;
                }
            }

            // -----------------------------------
            // STEP 3: Extract number from street
            // -----------------------------------
            var numberMatch = Regex.Match(
                streetPart,
                @"\d+\s?[A-Za-zΑ-Ωα-ω\-]*$"
            );

            string street = streetPart;

            if (numberMatch.Success)
            {
                result.Number = numberMatch.Value.Trim();
                street = streetPart[..numberMatch.Index].Trim();
            }

            // -----------------------------------
            // STEP 4: Final cleanup
            // -----------------------------------
            result.Street = NormalizeStreet(street);

            return result;
        }

        private static string NormalizeStreet(string street)
        {
            if (string.IsNullOrWhiteSpace(street))
                return street;

            return Regex.Replace(street, @"\s+", " ").Trim();
        }
    }
}
