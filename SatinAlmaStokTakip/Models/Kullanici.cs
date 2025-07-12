namespace SatinAlmaStokTakip.Models
{
    public class Kullanici
    {
        public int ID { get; set; }
        public string? AdSoyad { get; set; }
        public string? KullaniciAdi { get; set; }
        public string? Sifre { get; set; }
        public string? Rol { get; set; }  // örnek: Admin, Yetkili, Personel
        public string? Email { get; set; } // E-posta adresi
        public bool IsActive { get; set; } = true; // Pasif kayıt için
    }
}
