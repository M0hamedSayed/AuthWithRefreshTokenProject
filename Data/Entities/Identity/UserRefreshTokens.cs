using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Identity
{
    [Table("user_refresh_tokens")]
    [Comment("Handle refresh Tokens for users")]
    [Index(nameof(UserId), IsUnique = false)]
    public class UserRefreshTokens
    {
        [Key]
        [Column("token_id")]
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("access_token")]
        public required string AccessToken { get; set; }
        [Column("refresh_token")]
        public required string RefreshToken { get; set; }
        [Column("is_used")]
        public bool IsUsed { get; set; } = false;
        [Column("is_revoked")]
        public bool IsRevoked { get; set; } = false;
        [Column("added_time")]
        public DateTime AddedTime { get; set; } = DateTime.UtcNow;
        [Column("expiry_date")]
        public DateTime ExpiryDate { get; set; }

        // user metadata
        [Column("user_agent",TypeName = "nvarchar(300)")]
        public string? UserAgent { get; set; }
        [MaxLength(20)]
        public string? IP { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }
        [MaxLength(10)]
        [Column("country_code")]
        public string? CountryCode { get; set; }
        [MaxLength(100)]
        public string? City { get; set;}
        [MaxLength(100)]
        [Column("time_zone")]
        public string? TimeZone { get; set; }
        [Precision(8, 6)]
        [Column("location_lat")]
        public decimal? LocationLat { get; set; }
        [Precision(9, 6)]
        [Column("location_lng")]
        public decimal? LocationLng { get; set; }


        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(ApplicationUser.userRefreshTokens))]
        public virtual ApplicationUser? user { get; set; }
    }
}
