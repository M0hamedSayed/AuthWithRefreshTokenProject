using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Interfaces
{
    public interface ITableCreation
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
