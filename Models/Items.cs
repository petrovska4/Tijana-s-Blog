namespace TijanasBlog.Models
{
    public class Items
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public int Price { get; set; }
        public int BrandId { get; set; }
        public string? Image { get; set; }
        public Brands? Brand { get; set; }
        public ICollection<ItemsShops>? Shops { get; set; }
        public ICollection<Reviews>? Reviews { get; set; }
    }
}
