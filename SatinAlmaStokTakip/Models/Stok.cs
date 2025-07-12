namespace SatinAlmaStokTakip.Models
{
    public class Stok
    {
        public int ID { get; set; }
        public string? MalzemeAdi { get; set; }
        public int Adet { get; set; }
        public DateTime GirisTarihi { get; set; }
        public decimal BirimFiyat { get; set; } // Birim fiyat
        public bool IsActive { get; set; } = true; // Pasif kayıt için
    }
}
