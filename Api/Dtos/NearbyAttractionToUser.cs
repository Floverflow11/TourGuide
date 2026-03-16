namespace TourGuide.Dtos;

public record NearbyAttractionToUser(
    string Name,
    double Longitude,
    double Latitude,
    double UserLongitude,
    double UserLatitude,
    double DistanceMiles,
    int RewardPoints);