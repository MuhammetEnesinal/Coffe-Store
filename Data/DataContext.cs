
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoffeStore.Models
{
    public class DataContext : IdentityDbContext<AppUser,AppRole,int>
    {
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<Cart>  Carts { get; set; }
        public DbSet<Slider> Sliders { get; set; }  
         public DbSet<Kategori> Kategoriler { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
        modelBuilder.Entity<Kategori>().HasData(
            new List<Kategori> {
                new Kategori {Id=1, KategoriAdi="Latte", Url="latte"},
                new Kategori {Id=2, KategoriAdi="Espresso", Url="espresso"},
                new Kategori {Id=3, KategoriAdi="Americano", Url="americano"},
                new Kategori {Id=4, KategoriAdi="Mocha", Url="mocha"},
                new Kategori {Id=5, KategoriAdi="Kozmetik", Url="kozmetik"},
                new Kategori {Id=6, KategoriAdi="Cappuccino", Url="cappuccino"},
                new Kategori {Id=7, KategoriAdi="Kategori 2", Url="kategori-2"},
                new Kategori {Id=8, KategoriAdi="Kategori 3", Url="kategori-3"},
                new Kategori {Id=9, KategoriAdi="Kategori 4", Url="kategori-4"},
                new Kategori {Id=10, KategoriAdi="Kategori 5", Url="kategori-5"},
            }
        );
            // Urun entity configuration
            modelBuilder.Entity<Urun>().HasData(
                new Urun
                {
                    Id = 1,
                    UrunAdi = "Espresso",
                    Aciklama = "Yoğun ve aromatik İtalyan kahvesi.",
                    Fiyat = 15.0,
                    ResimUrl = "espresso.jpg",
                    KategoriId = 2,
                    Aktif = true,

                },


                new Urun
                {
                    Id = 2,
                    UrunAdi = "Cappuccino",
                    Aciklama = "Espresso, buharda ısıtılmış süt ve köpük karışımı.",
                    Fiyat = 20.0,
                    ResimUrl = "cappuccino.jpg",
                    KategoriId = 6,
                    Aktif = true,

                },
                new Urun
                {
                    Id = 3,
                    UrunAdi = "Latte",
                    Aciklama = "Espresso ve bol miktarda buharda ısıtılmış süt.",
                    Fiyat = 22.0,
                    ResimUrl = "latte.jpg",
                    KategoriId = 1,
                    Aktif = true,

                },
                new Urun
                {
                    Id = 4,
                    UrunAdi = "Mocha",
                    Aciklama = "Espresso, çikolata şurubu ve buharda ısıtılmış süt.",
                    Fiyat = 25.0,
                    ResimUrl = "mocha.jpg",
                    KategoriId = 4,
                    Aktif = true,

                },
                new Urun
                {
                    Id = 5,
                    UrunAdi = "Americano",
                    Aciklama = "Espresso ve sıcak su karışımı.",
                    Fiyat = 18.0,
                    ResimUrl = "americano.jpg",
                    KategoriId = 3,
                    Aktif = true,

                },

                 new Urun
                 {
                     Id = 6,
                     UrunAdi = "Espresso",
                     Aciklama = "Yoğun ve aromatik İtalyan kahvesi.",
                     Fiyat = 15.0,
                     ResimUrl = "espresso.jpg",
                     KategoriId = 2,
                     Aktif = true,

                 },

                    new Urun
                    {
                        Id = 7,
                        UrunAdi = "Espresso",
                        Aciklama = "Yoğun ve aromatik İtalyan kahvesi.",
                        Fiyat = 15.0,
                        ResimUrl = "espresso.jpg",
                        KategoriId = 2,
                        Aktif = true,

                    }
            );

            modelBuilder.Entity<Slider>().HasData(
                new List<Slider>
                {

                    new Slider {Id=1, Baslik="Kahvenin En İyisi", Aciklama="Taze çekilmiş kahve çekirdekleriyle hazırlanan enfes lezzetler.", ResimUrl="filreKahve.jpg", Aktif = true},
                    new Slider {Id=2, Baslik="Günün Her Anı İçin", Aciklama="Sabah kahvaltısından akşam sohbetlerine kadar her anınıza eşlik eder.", ResimUrl="americano.jpg", Aktif = true},
                    new Slider {Id=3, Baslik="Doğal ve Organik", Aciklama="Doğal yöntemlerle yetiştirilmiş organik kahve çekirdekleri kullanılır.", ResimUrl="espresso.jpg", Aktif = true}
                });
        }
    }
}