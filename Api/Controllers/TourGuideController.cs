using GpsUtil.Location;
using Microsoft.AspNetCore.Mvc;
using TourGuide.Dtos;
using TourGuide.Services.Interfaces;
using TourGuide.Users;
using TripPricer;

namespace TourGuide.Controllers;

[ApiController]
[Route("[controller]")]
public class TourGuideController : ControllerBase
{
    private readonly ITourGuideService _tourGuideService;
    private readonly IRewardsService _rewardsService;

    public TourGuideController(ITourGuideService tourGuideService, IRewardsService rewardsService)
    {
        _tourGuideService = tourGuideService;
        _rewardsService = rewardsService;
    }

    [HttpGet("getLocation")]
    public ActionResult<VisitedLocation> GetLocation([FromQuery] string userName)
    {
        var location = _tourGuideService.GetUserLocation(GetUser(userName));
        return Ok(location);
    }
    
    [HttpGet("getNearbyAttractions")]
    public ActionResult<List<NearbyAttractionToUser>> GetNearbyAttractions([FromQuery] string userName)
    {
        var user = _tourGuideService.GetUser(userName);
        var visitedLocation = _tourGuideService.GetUserLocation(user);
        var attractions = _tourGuideService.GetNearByAttractions(visitedLocation);
        var userLocation = visitedLocation.Location;

        var dtos = new List<NearbyAttractionToUser>();

        foreach (var attraction in attractions)
        {
            var distance = _rewardsService.GetDistance(attraction, userLocation);
            var rewardPoints = _rewardsService.GetRewardPoints(attraction, user);

            dtos.Add(new NearbyAttractionToUser(attraction.AttractionName, attraction.Longitude, attraction.Latitude,
                userLocation.Longitude, userLocation.Latitude, distance, rewardPoints));
        }

        return Ok(dtos);
    }

    [HttpGet("getRewards")]
    public ActionResult<List<UserReward>> GetRewards([FromQuery] string userName)
    {
        var rewards = _tourGuideService.GetUserRewards(GetUser(userName));
        return Ok(rewards);
    }

    [HttpGet("getTripDeals")]
    public ActionResult<List<Provider>> GetTripDeals([FromQuery] string userName)
    {
        var deals = _tourGuideService.GetTripDeals(GetUser(userName));
        return Ok(deals);
    }

    private User GetUser(string userName)
    {
        return _tourGuideService.GetUser(userName);
    }
}