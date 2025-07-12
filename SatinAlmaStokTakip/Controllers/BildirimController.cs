using Microsoft.AspNetCore.Mvc;
using SatinAlmaStokTakip.Models;

namespace SatinAlmaStokTakip.Controllers
{
    public class BildirimController : Controller
    {
        private readonly VeritabaniContext _context;

        public BildirimController(VeritabaniContext context)
        {
            _context = context;
        }

        [HttpGet]
        public JsonResult GetBildirimler()
        {
            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            if (string.IsNullOrEmpty(kullaniciAdi))
                return Json(new { bildirimler = new List<object>() });

            var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.KullaniciAdi == kullaniciAdi);
            if (kullanici == null)
                return Json(new { bildirimler = new List<object>() });

            var bildirimler = _context.Bildirimler
                .Where(b => b.KullaniciID == kullanici.ID && b.IsActive)
                .OrderByDescending(b => b.Tarih)
                .Take(10)
                .Select(b => new {
                    b.ID,
                    b.Baslik,
                    b.Mesaj,
                    b.Tip,
                    b.Tarih,
                    b.Okundu,
                    b.Link
                })
                .ToList();

            var okunmamisSayisi = _context.Bildirimler
                .Count(b => b.KullaniciID == kullanici.ID && b.IsActive && !b.Okundu);

            return Json(new { bildirimler, okunmamisSayisi });
        }

        [HttpPost]
        public JsonResult OkunduIsaretle(int id)
        {
            var bildirim = _context.Bildirimler.FirstOrDefault(b => b.ID == id);
            if (bildirim != null)
            {
                bildirim.Okundu = true;
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public JsonResult TumunuOkunduIsaretle()
        {
            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            if (string.IsNullOrEmpty(kullaniciAdi))
                return Json(new { success = false });

            var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.KullaniciAdi == kullaniciAdi);
            if (kullanici == null)
                return Json(new { success = false });

            var bildirimler = _context.Bildirimler
                .Where(b => b.KullaniciID == kullanici.ID && b.IsActive && !b.Okundu);

            foreach (var bildirim in bildirimler)
            {
                bildirim.Okundu = true;
            }

            _context.SaveChanges();
            return Json(new { success = true });
        }

        // Statik bildirim oluşturma metodu
        public static void BildirimOlustur(VeritabaniContext context, int kullaniciID, string baslik, string mesaj, string tip = "info", string? link = null)
        {
            var bildirim = new Bildirim
            {
                KullaniciID = kullaniciID,
                Baslik = baslik,
                Mesaj = mesaj,
                Tip = tip,
                Tarih = DateTime.Now,
                Link = link,
                IsActive = true
            };

            context.Bildirimler.Add(bildirim);
            context.SaveChanges();
        }
    }
} 