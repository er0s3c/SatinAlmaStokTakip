using Microsoft.EntityFrameworkCore;

namespace SatinAlmaStokTakip.Models
{
    public class VeritabaniContext : DbContext
    {
        public VeritabaniContext(DbContextOptions<VeritabaniContext> options) : base(options) { }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Talep> Talepler { get; set; }
        public DbSet<Teklif> Teklifler { get; set; }
        public DbSet<Fatura> Faturalar { get; set; }
        public DbSet<Stok> Stoklar { get; set; }
        public DbSet<Tuketim> Tuketimler { get; set; }
        public DbSet<Log> Loglar { get; set; }
        public DbSet<Yorum> Yorumlar { get; set; }
        public DbSet<Bildirim> Bildirimler { get; set; }

    }
}
