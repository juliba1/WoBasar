using Supabase;
using WoBasar.Shared;
using WoBasar.Shared.Models;

namespace WoBasar.API.Database
{
    public class SupabaseConnector
    {
        private readonly Client _supabase;
        private readonly bool _hasWriteAccess;

        public SupabaseConnector(IConfiguration configuration)
        {
            var supabaseUrl = configuration["Supabase:Url"];
            var supabaseKey = configuration["Supabase:ServiceRoleKey"];

            if (string.IsNullOrWhiteSpace(supabaseKey))
            {
                supabaseKey = configuration["Supabase:Key"];
            }

            if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey))
            {
                throw new ArgumentNullException("Supabase URL or Key is not configured.");
            }

            _hasWriteAccess = !string.IsNullOrWhiteSpace(configuration["Supabase:ServiceRoleKey"]);

            // Supabase Client initialisieren
            _supabase = new Client(supabaseUrl, supabaseKey, new SupabaseOptions{});
        }

        public async Task<List<ListingModel>> GetListingsAsync()
        {
            try
            {
                var response = await _supabase
                    .From<ListingModel>()
                    .Get();

                return response.Models;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching listings from Supabase: {ex.Message}");
                throw;
            }
        }

        public async Task<List<ListingModel>> GetListingsByUserInitialsAsync(string userInitials)
        {
            var response = await _supabase
                .From<ListingModel>()
                .Where(x => x.UserInitials == userInitials)
                .Get();

            return response.Models;
        }

        public async Task<List<string>> GetDistinctCategoriesAsync()
        {
            var response = await _supabase
                .From<ListingModel>()
                .Select(x => new object[] { x.Category })
                .Get();

            return response.Models
                .Select(x => x.Category)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();
        }

        public async Task<ListingModel?> GetListingByIdAsync(Guid id)
        {
            var response = await _supabase
                .From<ListingModel>()
                .Where(x => x.Id == id)
                .Get();

            return response.Models.FirstOrDefault();
        }

        public async Task<ListingModel> CreateListingAsync(ListingUpsertModel request)
        {
            EnsureWriteAccessConfigured();

            var now = DateTime.UtcNow;
            var listing = new ListingModel
            {
                Title = request.Title.Trim(),
                Category = request.Category.Trim(),
                FilterTag = request.FilterTag.Trim(),
                Emoji = request.Emoji.Trim(),
                ThumbClass = string.IsNullOrWhiteSpace(request.ThumbClass) ? "wo-thumb-default" : request.ThumbClass.Trim(),
                PriceRaw = request.PriceRaw,
                Price = $"{request.PriceRaw} €",
                PriceSuffix = string.IsNullOrWhiteSpace(request.PriceSuffix) ? null : request.PriceSuffix.Trim(),
                Badge = string.IsNullOrWhiteSpace(request.Badge) ? null : request.Badge.Trim(),
                BadgeClass = string.IsNullOrWhiteSpace(request.BadgeClass) ? null : request.BadgeClass.Trim(),
                Username = request.Username.Trim(),
                UserInitials = request.UserInitials.Trim(),
                Location = request.Location.Trim(),
                AvatarBg = string.IsNullOrWhiteSpace(request.AvatarBg) ? "#EEF2FF" : request.AvatarBg.Trim(),
                AvatarColor = string.IsNullOrWhiteSpace(request.AvatarColor) ? "#4F46E5" : request.AvatarColor.Trim(),
                Saved = false,
                CreatedAt = now,
                UpdatedAt = now
            };

            var response = await _supabase.From<ListingModel>().Insert(listing);
            return response.Models.First();
        }

        public async Task<ListingModel?> UpdateListingAsync(Guid id, ListingUpsertModel request)
        {
            EnsureWriteAccessConfigured();

            var existingResponse = await _supabase
                .From<ListingModel>()
                .Where(x => x.Id == id)
                .Get();

            var existing = existingResponse.Models.FirstOrDefault();
            if (existing is null)
            {
                return null;
            }

            existing.Title = request.Title.Trim();
            existing.Category = request.Category.Trim();
            existing.FilterTag = request.FilterTag.Trim();
            existing.Emoji = request.Emoji.Trim();
            existing.ThumbClass = string.IsNullOrWhiteSpace(request.ThumbClass) ? existing.ThumbClass : request.ThumbClass.Trim();
            existing.PriceRaw = request.PriceRaw;
            existing.Price = $"{request.PriceRaw} €";
            existing.PriceSuffix = string.IsNullOrWhiteSpace(request.PriceSuffix) ? null : request.PriceSuffix.Trim();
            existing.Badge = string.IsNullOrWhiteSpace(request.Badge) ? null : request.Badge.Trim();
            existing.BadgeClass = string.IsNullOrWhiteSpace(request.BadgeClass) ? null : request.BadgeClass.Trim();
            existing.Username = request.Username.Trim();
            existing.UserInitials = request.UserInitials.Trim();
            existing.Location = request.Location.Trim();
            existing.AvatarBg = string.IsNullOrWhiteSpace(request.AvatarBg) ? existing.AvatarBg : request.AvatarBg.Trim();
            existing.AvatarColor = string.IsNullOrWhiteSpace(request.AvatarColor) ? existing.AvatarColor : request.AvatarColor.Trim();
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _supabase.From<ListingModel>().Update(existing);
            return updated.Models.FirstOrDefault();
        }

        public async Task<bool> DeleteListingAsync(Guid id)
        {
            EnsureWriteAccessConfigured();

            var existingResponse = await _supabase
                .From<ListingModel>()
                .Where(x => x.Id == id)
                .Get();

            var existing = existingResponse.Models.FirstOrDefault();
            if (existing is null)
            {
                return false;
            }

            await _supabase.From<ListingModel>().Delete(existing);
            return true;
        }

        private void EnsureWriteAccessConfigured()
        {
            if (_hasWriteAccess)
            {
                return;
            }

            throw new InvalidOperationException(
                "Write access denied by Supabase RLS. Configure Supabase:ServiceRoleKey in API appsettings or environment variables to allow POST/PUT/DELETE.");
        }
    }
}
