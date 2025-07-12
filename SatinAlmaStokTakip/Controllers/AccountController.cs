using Microsoft.AspNetCore.Mvc;
using SatinAlmaStokTakip.Models;
using System.Linq;

namespace SatinAlmaStokTakip.Controllers
{
    public class AccountController : Controller
    {
        private readonly VeritabaniContext _context;

        public AccountController(VeritabaniContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string kullaniciAdi, string sifre)
        {
            var kullanici = _context.Kullanicilar
                .FirstOrDefault(k => k.KullaniciAdi == kullaniciAdi && k.Sifre == sifre);

            if (kullanici != null)
            {
                HttpContext.Session.SetString("KullaniciAdi", kullanici.KullaniciAdi ?? "");
                HttpContext.Session.SetString("Rol", kullanici.Rol ?? "Personel");
                
                LogController.LogEkle(_context, kullaniciAdi, "Sisteme giriş yapıldı");
                
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Hata = "Kullanıcı adı veya şifre hatalı.";
            return View();
        }

        public IActionResult Logout()
        {
            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            if (!string.IsNullOrEmpty(kullaniciAdi))
            {
                LogController.LogEkle(_context, kullaniciAdi, "Sistemden çıkış yapıldı");
            }
            
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
