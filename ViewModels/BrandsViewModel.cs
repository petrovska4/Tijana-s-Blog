using TijanasBlog.Models;

namespace TijanasBlog.ViewModels
{
    public class BrandsViewModel
    {
        public string? SearchName { get; set; }
        public IList<Brands>? Brands { get; set; }
    }
}
