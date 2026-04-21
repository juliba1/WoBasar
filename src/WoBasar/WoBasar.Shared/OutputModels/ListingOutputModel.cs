using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoBasar.Shared
{
    public class ListingOutputModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string Category { get; set; } = "";
        public string FilterTag { get; set; } = "Alle";
        public string Emoji { get; set; } = "";
        public string ThumbClass { get; set; } = "";
        public string Price { get; set; } = "";
        public string? PriceSuffix { get; set; }
        public string? Badge { get; set; }
        public string? BadgeClass { get; set; }
        public string Username { get; set; } = "";
        public string UserInitials { get; set; } = "";
        public string Location { get; set; } = "";
        public string AvatarBg { get; set; } = "#EEF2FF";
        public string AvatarColor { get; set; } = "#4F46E5";
        public bool Saved { get; set; }
        public int PriceRaw { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
