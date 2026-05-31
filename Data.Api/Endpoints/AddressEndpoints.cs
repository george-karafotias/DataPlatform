using Data.Core.Address.Interfaces;
using Data.Core.Address.Models;
using Data.Core.Shared.Contracts;

namespace Data.Api.Endpoints
{
    public static class AddressEndpoints
    {
        public static RouteGroupBuilder MapAddressEndpoints(
            this RouteGroupBuilder group)
        {
            var addressGroup = group.MapGroup("/address")
                                    .WithTags("Address");

            addressGroup.MapPost("/parse",
            (AddressRequest request, IAddressParser parser) =>
            {
                if (string.IsNullOrWhiteSpace(request.Address))
                {
                    return Results.BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Error = "Address cannot be empty"
                    });
                }

                var result = parser.Parse(request.Address);

                return Results.Ok(new ApiResponse<AddressResponse>
                {
                    Success = true,
                    Data = result
                });
            })
            .WithName("ParseAddress")
            .WithSummary("Parses a Greek address into structured components")
            .WithDescription("Extracts street, number, city and postal code from a free-form Greek address string.");

            return group;
        }
    }
}