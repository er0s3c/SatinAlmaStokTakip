using Microsoft.AspNetCore.Mvc;
using SatinAlmaStokTakip.Models;

namespace SatinAlmaStokTakip.Controllers
{
    public class YorumController : Controller
    {
        private readonly VeritabaniContext _context;

        public YorumController(VeritabaniContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Ekle(string tip, int kayitId, string icerik)
        {
            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.KullaniciAdi == kullaniciAdi);
            if (kullanici == null)
                return Unauthorized();

            var yorum = new Yorum
            {
                Tip = tip, // "Talep" veya "Teklif"
                KayitID = kayitId,
                Icerik = icerik,
                Tarih = DateTime.Now,
                KullaniciID = kullanici.ID,
                IsActive = true
            };
            _context.Yorumlar.Add(yorum);
            _context.SaveChanges();

            if (tip == "Talep")
                return RedirectToAction("Detay", "Talep", new { id = kayitId });
            else if (tip == "Teklif")
                return RedirectToAction("Detay", "Teklif", new { id = kayitId });
            else
                return RedirectToAction("Index", "Home");
        }
    }
} 