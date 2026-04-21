using Supabase;
using WoBasar.Shared;

namespace WoBasar.API.Database
{
    public class SupabaseConnector
    {
        private readonly Client _supabase;

        public SupabaseConnector(IConfiguration configuration)
        {
            var supabaseUrl = configuration["Supabase:Url"];
            var supabaseKey = configuration["Supabase:Key"];

            if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey))
            {
                throw new ArgumentNullException("Supabase URL or Key is not configured.");
            }

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
    }
}
