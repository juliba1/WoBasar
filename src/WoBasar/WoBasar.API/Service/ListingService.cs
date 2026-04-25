using WoBasar.API.Database;
using WoBasar.Shared;
using WoBasar.Shared.Models;

namespace WoBasar.API.Service
{
    public class ListingService
    {
        private readonly SupabaseConnector _repository;

        public ListingService(SupabaseConnector repository)
        {
            _repository = repository;
        }

        public async Task<List<ListingOutputModel>> GetAllListingsAsync()
        {
            var listingsFromDb = await _repository.GetListingsAsync();

            return MapToOutput(listingsFromDb);
        }

        public async Task<List<ListingOutputModel>> GetMyListingsAsync(string userInitials)
        {
            var listingsFromDb = await _repository.GetListingsByUserInitialsAsync(userInitials);

            return MapToOutput(listingsFromDb);
        }

        public Task<List<string>> GetDistinctCategoriesAsync()
        {
            return _repository.GetDistinctCategoriesAsync();
        }

        public async Task<ListingOutputModel?> GetListingByIdAsync(Guid id)
        {
            var listing = await _repository.GetListingByIdAsync(id);
            return listing is null ? null : MapToOutput(listing);
        }

        public async Task<ListingOutputModel> CreateListingAsync(ListingUpsertModel request)
        {
            var created = await _repository.CreateListingAsync(request);
            return MapToOutput(created);
        }

        public async Task<ListingOutputModel?> UpdateListingAsync(Guid id, ListingUpsertModel request)
        {
            var updated = await _repository.UpdateListingAsync(id, request);
            return updated is null ? null : MapToOutput(updated);
        }

        public Task<bool> DeleteListingAsync(Guid id)
        {
            return _repository.DeleteListingAsync(id);
        }

        private static List<ListingOutputModel> MapToOutput(IEnumerable<ListingModel> listingsFromDb)
        {
            return listingsFromDb.Select(MapToOutput).ToList();
        }

        private static ListingOutputModel MapToOutput(ListingModel l)
        {
            return new ListingOutputModel
            {
                Id = l.Id,
                Title = l.Title,
                Category = l.Category,
                FilterTag = l.FilterTag,
                Emoji = l.Emoji,
                ThumbClass = l.ThumbClass,
                Price = l.Price,
                PriceSuffix = l.PriceSuffix,
                Badge = l.Badge,
                BadgeClass = l.BadgeClass,
                Username = l.Username,
                UserInitials = l.UserInitials,
                Location = l.Location,
                AvatarBg = l.AvatarBg,
                AvatarColor = l.AvatarColor,
                Saved = l.Saved,
                PriceRaw = l.PriceRaw,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt
            };
        }
    }
}
