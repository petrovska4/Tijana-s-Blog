namespace TijanasBlog.Models
{
    public class ItemsShops
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int ShopId { get; set; }
        public Items? Item { get; set; }
        public Shops? Shop { get; set; }
    }
}
