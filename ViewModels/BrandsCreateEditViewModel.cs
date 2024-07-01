using TijanasBlog.Models;

namespace TijanasBlog.ViewModels
{
    public class BrandsCreateEditViewModel
    {
        public Brands? Brands { get; set; }
        public IFormFile? Image {  get; set; }
        public IFormFile? PdfFile { get; set; }
    }
}
