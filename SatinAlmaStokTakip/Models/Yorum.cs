namespace SatinAlmaStokTakip.Models
{
    public class Yorum
    {
        public int ID { get; set; }
        public string? Icerik { get; set; }
        public DateTime Tarih { get; set; }
        public int KullaniciID { get; set; }
        public string? Tip { get; set; } // "Talep" veya "Teklif"
        public int KayitID { get; set; } // Talep veya Teklif ID'si
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public Kullanici? Kullanici { get; set; }
    }
} 