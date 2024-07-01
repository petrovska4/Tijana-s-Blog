using static System.Reflection.Metadata.BlobBuilder;
using System.ComponentModel.DataAnnotations;

namespace TijanasBlog.Models
{
    public class Reviews
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string? AppUser { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
        public Items? Item { get; set; }
    }
}
