namespace SatinAlmaStokTakip.Models
{
    public class Teklif
    {
        public int ID { get; set; }
        public int TalepID { get; set; }
        public string? FirmaAdi { get; set; }
        public decimal Fiyat { get; set; }
        public string? Notlar { get; set; }
        public string? OnayDurumu { get; set; } // "Bekliyor", "Onaylandı", "Reddedildi"
        public DateTime? OnayTarihi { get; set; }
        public int? OnaylayanKullaniciID { get; set; }
        public string? OnayNotu { get; set; }
        public string? DosyaYolu { get; set; } // Yüklenen dosyanın yolu
        public bool IsActive { get; set; } = true; // Pasif kayıt için

        public Talep? Talep { get; set; }
        public Kullanici? OnaylayanKullanici { get; set; }
        public List<Yorum>? Yorumlar { get; set; }
    }
}
