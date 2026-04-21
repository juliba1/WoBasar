using Microsoft.AspNetCore.Mvc;
using WoBasar.API.Service;
using WoBasar.Shared;
using WoBasar.Shared.Models;

namespace WoBasar.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListingController : ControllerBase
    {
        private readonly ListingService _listingService;

        public ListingController(ListingService listingService)
        {
            _listingService = listingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListingOutputModel>>> GetListings()
        {
            try
            {
                var listings = await _listingService.GetAllListingsAsync();
                return Ok(listings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
