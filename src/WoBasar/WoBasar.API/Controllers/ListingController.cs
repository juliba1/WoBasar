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

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ListingOutputModel>> GetListing(Guid id)
        {
            try
            {
                var listing = await _listingService.GetListingByIdAsync(id);
                if (listing is null)
                {
                    return NotFound();
                }

                return Ok(listing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("mine")]
        public async Task<ActionResult<IEnumerable<ListingOutputModel>>> GetMyListings([FromQuery] string userInitials)
        {
            if (string.IsNullOrWhiteSpace(userInitials))
            {
                return BadRequest("The query parameter 'userInitials' is required.");
            }

            try
            {
                var listings = await _listingService.GetMyListingsAsync(userInitials.Trim());
                return Ok(listings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<string>>> GetDistinctCategories()
        {
            try
            {
                var categories = await _listingService.GetDistinctCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ListingOutputModel>> CreateListing([FromBody] ListingUpsertModel request)
        {
            var validationErrors = ValidateUpsertRequest(request);
            if (validationErrors.Count > 0)
            {
                return BadRequest(new ValidationProblemDetails(validationErrors));
            }

            try
            {
                var created = await _listingService.CreateListingAsync(request);
                return CreatedAtAction(nameof(GetListings), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ListingOutputModel>> UpdateListing(Guid id, [FromBody] ListingUpsertModel request)
        {
            var validationErrors = ValidateUpsertRequest(request);
            if (validationErrors.Count > 0)
            {
                return BadRequest(new ValidationProblemDetails(validationErrors));
            }

            try
            {
                var updated = await _listingService.UpdateListingAsync(id, request);
                if (updated is null)
                {
                    return NotFound();
                }

                return Ok(updated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteListing(Guid id)
        {
            try
            {
                var deleted = await _listingService.DeleteListingAsync(id);
                if (!deleted)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        private static Dictionary<string, string[]> ValidateUpsertRequest(ListingUpsertModel request)
        {
            var errors = new Dictionary<string, string[]>();

            if (request is null)
            {
                errors["request"] = new[] { "Request body is required." };
                return errors;
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                errors[nameof(request.Title)] = new[] { "Title is required." };
            }

            if (string.IsNullOrWhiteSpace(request.Category))
            {
                errors[nameof(request.Category)] = new[] { "Category is required." };
            }

            if (string.IsNullOrWhiteSpace(request.FilterTag))
            {
                errors[nameof(request.FilterTag)] = new[] { "FilterTag is required (maps to filter_tag)." };
            }

            if (string.IsNullOrWhiteSpace(request.Emoji))
            {
                errors[nameof(request.Emoji)] = new[] { "Emoji is required." };
            }

            if (request.PriceRaw < 0)
            {
                errors[nameof(request.PriceRaw)] = new[] { "PriceRaw must be zero or positive." };
            }

            if (string.IsNullOrWhiteSpace(request.Username))
            {
                errors[nameof(request.Username)] = new[] { "Username is required." };
            }

            if (string.IsNullOrWhiteSpace(request.UserInitials))
            {
                errors[nameof(request.UserInitials)] = new[] { "UserInitials is required." };
            }

            if (string.IsNullOrWhiteSpace(request.Location))
            {
                errors[nameof(request.Location)] = new[] { "Location is required." };
            }

            return errors;
        }
    }
}
