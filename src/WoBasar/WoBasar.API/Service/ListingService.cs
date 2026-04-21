using WoBasar.API.Database;
using WoBasar.Shared;

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

            var listingOutput = listingsFromDb.Select(l => new ListingOutputModel
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
            }).ToList();


            return listingOutput;
        }

    }
}
