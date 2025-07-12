namespace SatinAlmaStokTakip.Models
{
    public class Bildirim
    {
        public int ID { get; set; }
        public int KullaniciID { get; set; }
        public string? Baslik { get; set; }
        public string? Mesaj { get; set; }
        public string? Tip { get; set; } // "info", "warning", "success", "danger"
        public DateTime Tarih { get; set; }
        public bool Okundu { get; set; } = false;
        public string? Link { get; set; } // Tıklandığında gidilecek sayfa
        public bool IsActive { get; set; } = true;

        // Navigation property
        public Kullanici? Kullanici { get; set; }
    }
} 