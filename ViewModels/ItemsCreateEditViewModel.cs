using Microsoft.AspNetCore.Mvc.Rendering;
using TijanasBlog.Models;

namespace TijanasBlog.ViewModels
{
    public class ItemsCreateEditViewModel
    {
        public Items? Item {  get; set; }
        public IEnumerable<int>? SelectedShops { get; set; }
        public IEnumerable<SelectListItem>? ShopsList { get; set; }
        public IEnumerable<SelectListItem>? BrandsList { get; set; }
        public IFormFile? Image { get; set; }
    }
}
