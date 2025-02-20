using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Data.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int>, ISoftDeletable, ITableCreation
    {
        [MaxLength(100)]
        [Column("full_name")]
        public required String FullName { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; } = null;
        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;
        public DateTime? LastEmailConfirmSent { get; set; }
        public DateTime? LastEmailPasswordResetSent { get; set; }

        [InverseProperty(nameof(UserRefreshTokens.user))]
        public virtual ICollection<UserRefreshTokens>? userRefreshTokens { get; set; }
    }
}
