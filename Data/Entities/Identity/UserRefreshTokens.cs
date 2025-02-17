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
        public bool IsUsed { get; set; } = false;
        public bool IsRevoked { get; set; } = false;
        [Column("added_time")]
        public DateTime AddedTime { get; set; }
        [Column("expiry_date")]
        public DateTime ExpiryDate { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(ApplicationUser.userRefreshTokens))]
        public virtual ApplicationUser? user { get; set; }
    }
}
