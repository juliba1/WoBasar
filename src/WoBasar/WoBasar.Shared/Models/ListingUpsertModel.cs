namespace WoBasar.Shared.Models
{
    public class ListingUpsertModel
    {
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string FilterTag { get; set; } = string.Empty;
        public string Emoji { get; set; } = string.Empty;
        public int PriceRaw { get; set; }
        public string? PriceSuffix { get; set; }
        public string? Badge { get; set; }
        public string? BadgeClass { get; set; }
        public string? ThumbClass { get; set; }
        public string Username { get; set; } = string.Empty;
        public string UserInitials { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string? AvatarBg { get; set; }
        public string? AvatarColor { get; set; }
    }
}
