using Data.Core.Parking.Models;

namespace Data.Core.Parking.Interfaces;

public interface IParkingService
{
    IEnumerable<Models.Parking> GetAll();

    IEnumerable<Models.Parking> Search(string query);

    IEnumerable<NearbyParkingResponse> Nearby(
        double latitude,
        double longitude,
        double radiusMeters = 1000);
}