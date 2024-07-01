namespace TijanasBlog.Models
{
    public class Shops
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Logo { get; set; }
        public ICollection<ItemsShops>? Items { get; set; }
    }
}
