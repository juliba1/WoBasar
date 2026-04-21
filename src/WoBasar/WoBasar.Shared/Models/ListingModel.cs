using System;
using System.Collections.Generic;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WoBasar.Shared
{
    [Table("listings")] // Wichtig: Der genaue Name deiner Supabase-Tabelle
    public class ListingModel : BaseModel
    {
        [PrimaryKey("id", false)] // false, weil Supabase die UUID generiert
        public Guid Id { get; set; }

        [Column("title")]
        public string Title { get; set; } = "";

        [Column("category")]
        public string Category { get; set; } = "";

        [Column("filter_tag")]
        public string FilterTag { get; set; } = "Alle";

        [Column("emoji")]
        public string Emoji { get; set; } = "";

        [Column("thumb_class")]
        public string ThumbClass { get; set; } = "";

        [Column("price")]
        public string Price { get; set; } = "";

        [Column("price_suffix")]
        public string? PriceSuffix { get; set; }

        [Column("badge")]
        public string? Badge { get; set; }

        [Column("badge_class")]
        public string? BadgeClass { get; set; }

        [Column("username")]
        public string Username { get; set; } = "";

        [Column("user_initials")]
        public string UserInitials { get; set; } = "";

        [Column("location")]
        public string Location { get; set; } = "";

        [Column("avatar_bg")]
        public string AvatarBg { get; set; } = "#EEF2FF";

        [Column("avatar_color")]
        public string AvatarColor { get; set; } = "#4F46E5";

        [Column("saved")]
        public bool Saved { get; set; }

        [Column("price_raw")]
        public int PriceRaw { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
