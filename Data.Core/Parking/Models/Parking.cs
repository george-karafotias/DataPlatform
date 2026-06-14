namespace Data.Core.Parking.Models;

public class Parking
{
    public string Name { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string Address { get; set; } = string.Empty;

    public ParkingType ParkingType { get; set; }

    public bool IsFree { get; set; }

    public string? Phone { get; set; }

    public string? Website { get; set; }

    public string? Notes { get; set; }
}