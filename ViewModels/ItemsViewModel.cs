using Microsoft.AspNetCore.Mvc.Rendering;
using TijanasBlog.Models;

namespace TijanasBlog.ViewModels
{
    public class ItemsViewModel
    {
        public IList<Items>? Items { get; set; }
        public SelectList? Shops { get; set; }
        public string? ItemShop { get; set; }
        public string? SearchName { get; set; }
        public IList<Brands>? Brands { get; set; }
        public string? SearchBrand { get; set; }
        public string? SearchType { get; set; }

    }
}
