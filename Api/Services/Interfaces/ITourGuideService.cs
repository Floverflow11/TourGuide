using GpsUtil.Location;
using TourGuide.Dtos;
using TourGuide.Users;
using TourGuide.Utilities;
using TripPricer;

namespace TourGuide.Services.Interfaces
{
    public interface ITourGuideService
    {
        Tracker Tracker { get; }

        void AddUser(User user);
        List<User> GetAllUsers();
        List<Attraction> GetNearByAttractions(VisitedLocation visitedLocation);
        List<NearbyAttractionToUser> GetNearByAttractionsToUser(User user);
        List<Provider> GetTripDeals(User user);
        User GetUser(string userName);
        VisitedLocation GetUserLocation(User user);
        IReadOnlyList<UserReward> GetUserRewards(User user);
        VisitedLocation TrackUserLocation(User user);
    }
}