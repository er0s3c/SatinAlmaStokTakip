namespace SatinAlmaStokTakip.Models
{
    public class Talep
    {
        public int ID { get; set; }
        public int KullaniciID { get; set; }
        public DateTime TalepTarihi { get; set; }
        public string? Durum { get; set; } // "Oluşturuldu", "İncelemede", "Onaylandı", "Reddedildi"
        public string? OnayDurumu { get; set; } // "Bekliyor", "Onaylandı", "Reddedildi"
        public DateTime? OnayTarihi { get; set; }
        public int? OnaylayanKullaniciID { get; set; }
        public string? OnayNotu { get; set; }
        public string? DosyaYolu { get; set; } // Yüklenen dosyanın yolu
        public bool IsActive { get; set; } = true; // Pasif kayıt için

        public List<Teklif>? Teklifler { get; set; }
        public Kullanici? Kullanici { get; set; }
        public Kullanici? OnaylayanKullanici { get; set; }
        public List<Yorum>? Yorumlar { get; set; }
    }
}
