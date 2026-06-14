using Data.Core.Parking.Interfaces;
using Data.Core.Parking.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Data.Core.Parking.Services;

public class ParkingService : IParkingService
{
    private readonly List<Models.Parking> _parkings;

    public ParkingService()
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "Data",
            "thessparkings.json");

        if (!File.Exists(path))
        {
            _parkings = [];
            return;
        }

        var json = File.ReadAllText(path);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        options.Converters.Add(new JsonStringEnumConverter());

        _parkings = JsonSerializer.Deserialize<List<Models.Parking>>(
            json,
            options) ?? [];
    }

    IEnumerable<Models.Parking> IParkingService.GetAll()
    {
        return _parkings.OrderBy(p => p.Name);
    }

    IEnumerable<Models.Parking> IParkingService.Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Enumerable.Empty<Models.Parking>();

        query = query.Trim();

        return _parkings.Where(p =>
            p.Name.Contains(query,
                StringComparison.OrdinalIgnoreCase)
            ||
            p.Address.Contains(query,
                StringComparison.OrdinalIgnoreCase)
            ||
            p.ParkingType.ToString().Contains(query,
                StringComparison.OrdinalIgnoreCase)
        );
    }

    public IEnumerable<NearbyParkingResponse> Nearby(
    double latitude,
    double longitude,
    double radiusMeters = 1000)
    {
        return _parkings
            .Select(p => new NearbyParkingResponse
            {
                Parking = p,
                DistanceMeters = CalculateDistance(
                    latitude,
                    longitude,
                    p.Latitude,
                    p.Longitude)
            })
            .Where(x => x.DistanceMeters <= radiusMeters)
            .OrderBy(x => x.DistanceMeters);
    }

    private static double CalculateDistance(
    double lat1,
    double lon1,
    double lat2,
    double lon2)
    {
        const double R = 6371000; // meters

        var dLat = DegreesToRadians(lat2 - lat1);
        var dLon = DegreesToRadians(lon2 - lon1);

        var a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(DegreesToRadians(lat1)) *
            Math.Cos(DegreesToRadians(lat2)) *
            Math.Sin(dLon / 2) *
            Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(
            Math.Sqrt(a),
            Math.Sqrt(1 - a));

        return R * c;
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }
}