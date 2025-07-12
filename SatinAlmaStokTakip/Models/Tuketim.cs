using System;

namespace SatinAlmaStokTakip.Models
{
    public class Tuketim
    {
        public int ID { get; set; }
        public int StokID { get; set; }
        public int Miktar { get; set; }
        public DateTime Tarih { get; set; }
        public string? Aciklama { get; set; }
        public bool IsActive { get; set; } = true; // Pasif kayıt için

        public Stok? Stok { get; set; }
    }
}
