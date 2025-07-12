namespace SatinAlmaStokTakip.Models
{
    public class Fatura
    {
        public int ID { get; set; }
        public int TeklifID { get; set; }
        public string? FaturaNo { get; set; }
        public decimal Tutar { get; set; }
        public DateTime FaturaTarihi { get; set; }
        public bool IsActive { get; set; } = true; // Pasif kayıt için

        public Teklif? Teklif { get; set; }
    }
}
