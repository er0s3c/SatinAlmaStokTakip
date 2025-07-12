using Microsoft.AspNetCore.Mvc;
using SatinAlmaStokTakip.Models;
using System.Linq;

namespace SatinAlmaStokTakip.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly VeritabaniContext _context;

        public KullaniciController(VeritabaniContext context)
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

            var kullanicilar = _context.Kullanicilar.Where(k => k.IsActive).ToList();
            return View(kullanicilar);
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcı adının benzersiz olup olmadığını kontrol et
                var mevcutKullanici = _context.Kullanicilar.FirstOrDefault(k => k.KullaniciAdi == kullanici.KullaniciAdi);
                if (mevcutKullanici != null)
                {
                    ModelState.AddModelError("KullaniciAdi", "Bu kullanıcı adı zaten kullanılıyor.");
                    return View(kullanici);
                }

                _context.Kullanicilar.Add(kullanici);
                _context.SaveChanges();

                var adminKullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
                LogController.LogEkle(_context, adminKullaniciAdi, $"Yeni kullanıcı oluşturuldu: {kullanici.KullaniciAdi} - Rol: {kullanici.Rol}");

                return RedirectToAction(nameof(Index));
            }
            return View(kullanici);
        }

        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Index", "Home");

            var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.ID == id);
            if (kullanici == null)
                return NotFound();

            return View(kullanici);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Kullanici guncel)
        {
            if (ModelState.IsValid)
            {
                var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.ID == guncel.ID);
                if (kullanici == null)
                    return NotFound();

                // Kullanıcı adının benzersiz olup olmadığını kontrol et (kendisi hariç)
                var mevcutKullanici = _context.Kullanicilar.FirstOrDefault(k => k.KullaniciAdi == guncel.KullaniciAdi && k.ID != guncel.ID);
                if (mevcutKullanici != null)
                {
                    ModelState.AddModelError("KullaniciAdi", "Bu kullanıcı adı zaten kullanılıyor.");
                    return View(guncel);
                }

                kullanici.AdSoyad = guncel.AdSoyad;
                kullanici.KullaniciAdi = guncel.KullaniciAdi;
                kullanici.Sifre = guncel.Sifre;
                kullanici.Rol = guncel.Rol;

                _context.SaveChanges();

                var adminKullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
                LogController.LogEkle(_context, adminKullaniciAdi, $"Kullanıcı güncellendi: {kullanici.KullaniciAdi}");

                return RedirectToAction(nameof(Index));
            }
            return View(guncel);
        }

        public IActionResult Deactivate(int id)
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Admin")
                return RedirectToAction("Index", "Home");

            var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.ID == id);
            if (kullanici == null)
                return NotFound();

            var kullaniciAdi = kullanici.KullaniciAdi;
            kullanici.IsActive = false;
            _context.SaveChanges();

            var adminKullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            LogController.LogEkle(_context, adminKullaniciAdi, $"Kullanıcı pasifleştirildi: {kullaniciAdi}");

            return RedirectToAction(nameof(Index));
        }
    }
} 