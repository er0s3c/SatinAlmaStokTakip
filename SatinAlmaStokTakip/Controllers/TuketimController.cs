using Microsoft.AspNetCore.Mvc;
using SatinAlmaStokTakip.Models;
using System.Linq;
using SatinAlmaStokTakip.Services;

namespace SatinAlmaStokTakip.Controllers
{
    public class TuketimController : Controller
    {
        private readonly VeritabaniContext _context;
        private readonly IEmailService _emailService;

        public TuketimController(VeritabaniContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            var tuketimler = _context.Tuketimler.ToList();
            return View(tuketimler);
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            ViewBag.Stoklar = _context.Stoklar.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tuketim tuketim)
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            var stok = _context.Stoklar.FirstOrDefault(s => s.ID == tuketim.StokID);
            if (stok == null || stok.Adet < tuketim.Miktar)
            {
                ModelState.AddModelError("", "Yetersiz stok!");
                ViewBag.Stoklar = _context.Stoklar.ToList();
                return View(tuketim);
            }

            stok.Adet -= tuketim.Miktar;
            tuketim.Tarih = DateTime.Now;

            _context.Tuketimler.Add(tuketim);
            _context.SaveChanges();

            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            LogController.LogEkle(_context, kullaniciAdi, $"Stok tüketimi: {stok.MalzemeAdi} - Miktar: {tuketim.Miktar}");

            // E-posta bildirimi gönder
            await SendTuketimNotificationAsync(tuketim, stok);

            return RedirectToAction("Index");
        }

        private async Task SendTuketimNotificationAsync(Tuketim tuketim, Stok stok)
        {
            try
            {
                var adminKullanicilar = _context.Kullanicilar
                    .Where(k => k.Rol == "Admin" && k.IsActive)
                    .Select(k => new { k.Email })
                    .ToList();
                
                foreach (var admin in adminKullanicilar)
                {
                    var subject = "Stok Tüketimi Gerçekleşti";
                    var message = $"Stok tüketimi gerçekleşti:\n\n" +
                                 $"Malzeme: {stok.MalzemeAdi}\n" +
                                 $"Tüketilen Miktar: {tuketim.Miktar}\n" +
                                 $"Kalan Stok: {stok.Adet}\n" +
                                 $"Tarih: {tuketim.Tarih:dd.MM.yyyy HH:mm}";

                    await _emailService.SendNotificationAsync(admin.Email ?? "admin@example.com", subject, message);
                }
            }
            catch (Exception ex)
            {
                // E-posta gönderme hatası loglanabilir
                Console.WriteLine($"E-posta gönderme hatası: {ex.Message}");
            }
        }
    }
}
