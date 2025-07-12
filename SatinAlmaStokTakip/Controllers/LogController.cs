using Microsoft.AspNetCore.Mvc;
using SatinAlmaStokTakip.Models;
using System.Linq;

namespace SatinAlmaStokTakip.Controllers
{
    public class LogController : Controller
    {
        private readonly VeritabaniContext _context;

        public LogController(VeritabaniContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            var rol = HttpContext.Session.GetString("Rol");

            if (rol != "Admin")
                return RedirectToAction("Index", "Home");

            var loglar = _context.Loglar.OrderByDescending(l => l.Tarih).ToList();
            return View(loglar);
        }

        public static void LogEkle(VeritabaniContext context, string? kullaniciAdi, string islem)
        {
            if (string.IsNullOrEmpty(kullaniciAdi))
                kullaniciAdi = "Bilinmeyen Kullanıcı";

            var log = new Log
            {
                KullaniciAdi = kullaniciAdi,
                Islem = islem,
                Tarih = DateTime.Now
            };

            context.Loglar.Add(log);
            context.SaveChanges();
        }
    }
}
