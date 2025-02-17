using System.ComponentModel.DataAnnotations.Schema;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Data.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int>, ISoftDeletable
    {
        public required String FullName { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? UpdatedAt { get; set; } = null;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; } = null;

        [InverseProperty(nameof(UserRefreshTokens.user))]
        public virtual ICollection<UserRefreshTokens>? userRefreshTokens { get; set; }
    }
}
