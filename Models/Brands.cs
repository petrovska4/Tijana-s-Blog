namespace TijanasBlog.Models
{
    public class Brands
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsCrueltyFree { get; set; }
        public string? BasedWhere { get; set; }
        public string? PriceRange { get; set; }
        public string? Image {  get; set; }
        public string? DownloadCatalog { get; set; }
        public ICollection<Items>? Items { get; set; }
    }
}
