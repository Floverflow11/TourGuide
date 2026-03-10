using GpsUtil.Location;
using TripPricer;

namespace TourGuide.Users;

public class User
{
    public Guid UserId { get; }
    public string UserName { get; }
    public string PhoneNumber { get; set; }
    public string EmailAddress { get; set; }

    public DateTime LatestLocationTimestamp { get; set; }
    public IReadOnlyList<VisitedLocation> VisitedLocations
    {
        get
        {
            using (_lock.EnterScope())
            {
                return _visitedLocations.ToList();
            }
        }
    }
    public List<UserReward> UserRewards { get; } = new List<UserReward>();
    public UserPreferences UserPreferences { get; set; } = new UserPreferences();
    public List<Provider> TripDeals { get; set; } = new List<Provider>();
    
    private readonly List<VisitedLocation> _visitedLocations = [];
    private readonly Lock _lock = new();
    
    public User(Guid userId, string userName, string phoneNumber, string emailAddress)
    {
        UserId = userId;
        UserName = userName;
        PhoneNumber = phoneNumber;
        EmailAddress = emailAddress;
    }

    public void AddToVisitedLocations(VisitedLocation visitedLocation)
    {
        using (_lock.EnterScope())
        {
            _visitedLocations.Add(visitedLocation);
        }
    }

    public void ClearVisitedLocations()
    {
        using (_lock.EnterScope())
        {
            _visitedLocations.Clear();
        }
    }

    public void AddUserReward(UserReward userReward)
    {
        if (!UserRewards.Exists(r => r.Attraction.AttractionName == userReward.Attraction.AttractionName))
        {
            UserRewards.Add(userReward);
        }
    }

    public VisitedLocation GetLastVisitedLocation()
    {
        using (_lock.EnterScope())
        {
            return _visitedLocations[^1];
        }
    }
}