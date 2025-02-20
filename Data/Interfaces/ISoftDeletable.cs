
namespace Data.Interfaces
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set;  }
        public DateTime? DeletedAt { get; set; }
    }
}
