using Data.Core.Parking.Interfaces;

namespace Data.Api.Endpoints;

public static class ParkingEndpoints
{
    public static RouteGroupBuilder MapParkingEndpoints(
        this RouteGroupBuilder group)
    {
        var parking = group.MapGroup("/thessparking")
                           .WithTags("ThessParking");

        parking.MapGet("/",
            (IParkingService service) =>
            {
                return Results.Ok(service.GetAll());
            })
            .WithName("GetAllParkings")
            .WithSummary("Returns all parking areas in Thessaloniki")
            .WithDescription(
                "Returns all known parking locations including underground, surface and multi-storey parking.");

        parking.MapGet("/search",
            (string query,
             IParkingService service) =>
            {
                return Results.Ok(
                    service.Search(query));
            })
            .WithName("SearchParkings")
            .WithSummary("Search parking areas by keyword")
            .WithDescription(
                "Searches by name, address or parking type.");

        parking.MapGet("/nearby",
            (
                double lat,
                double lon,
                double radius,
                IParkingService service) =>
            {
                return Results.Ok(
                    service.Nearby(
                        lat,
                        lon,
                        radius));
            })
            .WithName("NearbyParkings")
            .WithSummary(
                "Returns nearby parking locations")
            .WithDescription(
                "Returns parking locations ordered by distance.");

        return group;
    }
}