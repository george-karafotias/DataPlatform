namespace Data.Core.Parking.Models;

public class NearbyParkingResponse
{
    public Parking Parking { get; set; } = default!;

    public double DistanceMeters { get; set; }
}