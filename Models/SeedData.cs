using Microsoft.EntityFrameworkCore;
using TijanasBlog.Models;
using TijanasBlog.Data;

namespace TijanasBlog.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new TijanasBlogContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<TijanasBlogContext>>()))
            {
                if (context.Items.Any())
                {
                    return;
                }

                context.Brands.AddRange(
                    new Brands
                    {
                        Name = "Rare Beauty",
                        Description = "Rare Beauty is a cosmetics brand founded by Selena Gomez in 2020, aimed at promoting self-acceptance and challenging beauty standards. The brand emphasizes inclusivity, offering a diverse range of products designed for all skin tones and types. Rare Beauty also supports mental health initiatives through its Rare Impact Fund, which aims to raise $100 million over ten years to increase access to mental health resources.",
                        IsCrueltyFree = true,
                        BasedWhere = "United States",
                        PriceRange = "mid-range",
                        Image = "https://images.ctfassets.net/o168wnc4ptip/5BABEb1JxP3Li4mhj3J64C/3febdade69b9c118954e77b96f923ca2/ESTABLISHED-Rare-Beauty_CASE_STUDY_01.jpg",
                        DownloadCatalog = "https://openlab.citytech.cuny.edu/teresina-eportfolio/files/2023/05/Inclusivity-in-Beauty_-Rare-Beauty-Teresina-Tomaino.pdf"
                    },
                    new Brands
                    {
                        Name = "Essence",
                        Description = "Essence is a well-known cosmetics brand celebrated for its wide selection of affordable and innovative beauty products. It caters to diverse makeup needs with a range that includes trendy and classic items. The brand's offerings are accessible and designed to enhance personal beauty routines without compromising quality or style.",
                        IsCrueltyFree = true,
                        BasedWhere = "Germany",
                        PriceRange = "drugstore",
                        Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRA4sa0vtl0uz0OpHSuZiIAd7vgerk2eYitjA&s",
                        DownloadCatalog = "https://www.cosnova.com/download/news/files/essence_20years_Word_EN.pdf"
                    },
                    new Brands
                    {
                        Name = "e.l.f.",
                        Description = "e.l.f. Cosmetics, launched in 2004, quickly became a favorite for its affordable and innovative makeup and skincare products. The brand is renowned for its commitment to inclusivity, offering a diverse range of shades and formulas suitable for various skin tones and types. e.l.f. Cosmetics continues to evolve, staying on-trend with new releases and maintaining a strong presence in the beauty industry.",
                        IsCrueltyFree = true,
                        BasedWhere = "United States",
                        PriceRange = "drugstore",
                        Image = "https://logowik.com/content/uploads/images/634913.webp",
                        DownloadCatalog = "https://www.stifel.com/prospectusfiles/PD_2406.pdf"
                    }
                    );
                context.SaveChanges();

                context.Items.AddRange(
                    new Items
                    {
                        Name = "Soft Pinch Liquid Blush",
                        Description = "A weightless, long-lasting liquid blush that blends and builds beautifully for a soft, healthy flush. Available in both matte and dewy finishes.",
                        Type = "Blush",
                        Price = 27,
                        BrandId = 1,
                        Image = "https://the-destino.com/wp-content/uploads/2023/06/C136E8A5-C789-497E-BA58-CF92013205A1.jpg"
                    },
                    new Items
                    {
                        Name = "Liquid Touch Weightless Foundation",
                        Description = "An innovative long-lasting foundation that combines the weightless feel of a serum with buildable medium coverage for truly breathable, layerable wear.",
                        Type = "Foundation",
                        Price = 35,
                        BrandId = 1,
                        Image = "https://www.byrdie.com/thmb/7X5RIP-_iUI4q4Qe5aQGbw2weVU=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/rare-beauty-liquid-touch-weightless-foundation-5-7aed1814ce94422c8a4433c94b89651c.jpg"
                    },
                    new Items
                    {
                        Name = "I LOVE EXTREME crazy volume mascara",
                        Description = "The i love extreme mascara’s \"crazy\" sister – for even more extreme volume! The deep-black, creamy texture covers each individual lash with colour and the extra-large elastomer brush provides sensational effects with the very first application.",
                        Type = "Mascara",
                        Price = 5,
                        BrandId = 2,
                        Image = "https://sahara.mk/wp-content/uploads/2021/12/mam_3705675_produktfoto_produkt_normal_1.3_646x646.jpg"
                    },
                    new Items
                    {
                        Name = "Power Grip Primer",
                        Description = "A gel-based, hydrating face primer that smooths skin while gripping your makeup.",
                        Type = "Primer",
                        Price = 10,
                        BrandId = 3,
                        Image = "https://beffshuff.com/wp-content/uploads/2023/05/DSC_0153.jpg"
                    },
                    new Items
                    {
                        Name = "Halo Glow Highlight Beauty Wand",
                        Description = "A liquid highlighter wand with a cushion-tip applicator to give your complexion a luminous glow.",
                        Type = "Highlighter",
                        Price = 9,
                        BrandId = 3,
                        Image = "https://theaestheticedge.com/wp-content/uploads/2023/06/09-elf-Halo-Glow-Blush-Beauty-Wand-magic-hour-1024x1024.webp"
                    }
                    );

                context.SaveChanges();

                context.Shops.AddRange(
                    new Shops
                    {
                        Name = "Walmart",
                        Logo = "https://static.wixstatic.com/media/e68264_5812e26b16fa4d47b9e3b4af1ee2f985~mv2.png/v1/crop/x_0,y_162,w_779,h_456/fill/w_420,h_246,al_c,q_95,enc_auto/Ulta%20Logo.png"
                    },
                    new Shops
                    {
                        Name = "Target",
                        Logo = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9a/Target_logo.svg/1541px-Target_logo.svg.png"
                    },
                    new Shops
                    {
                        Name = "Sephora",
                        Logo = "https://i.pinimg.com/736x/31/e1/0c/31e10c8a16291eb3f0227e5231b2a782.jpg"
                    }
                    );
                context.SaveChanges();

                context.ItemsShops.AddRange(
                    new ItemsShops
                    {
                        ItemId = 1,
                        ShopId = 1
                    },
                    new ItemsShops
                    {
                        ItemId = 1,
                        ShopId = 3
                    },
                    new ItemsShops
                    {
                        ItemId = 2,
                        ShopId = 1
                    },
                    new ItemsShops
                    {
                        ItemId = 2,
                        ShopId = 3
                    },
                    new ItemsShops
                    {
                        ItemId = 3,
                        ShopId = 1
                    },
                    new ItemsShops
                    {
                        ItemId = 3,
                        ShopId = 2
                    },
                    new ItemsShops
                    {
                        ItemId = 4,
                        ShopId = 1
                    },
                    new ItemsShops
                    {
                        ItemId = 4,
                        ShopId = 2
                    }
                    );
                context.SaveChanges();

                context.Reviews.AddRange();
                context.SaveChanges();

                context.Users.AddRange();
                context.SaveChanges();
            }
        }
    }
}
