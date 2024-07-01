using static System.Reflection.Metadata.BlobBuilder;
using System.ComponentModel.DataAnnotations;

namespace TijanasBlog.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string? AppUser { get; set; }
        public int ItemId { get; set; }
        public Items? Item { get; set; }
    }
}
