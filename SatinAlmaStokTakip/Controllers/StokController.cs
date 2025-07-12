using Microsoft.AspNetCore.Mvc;
using SatinAlmaStokTakip.Models;
using System.Linq;
using SatinAlmaStokTakip.Services;

namespace SatinAlmaStokTakip.Controllers
{
    public class StokController : Controller
    {
        private readonly VeritabaniContext _context;
        private readonly IEmailService _emailService;

        public StokController(VeritabaniContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            var stoklar = _context.Stoklar.Where(s => s.IsActive).ToList();
            return View(stoklar);
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Stok stok)
        {
            if (ModelState.IsValid)
            {
                stok.GirisTarihi = DateTime.Now;
                _context.Stoklar.Add(stok);
                _context.SaveChanges();

                var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
                LogController.LogEkle(_context, kullaniciAdi, $"Yeni stok eklendi: {stok.MalzemeAdi} - Miktar: {stok.Adet}");

                // E-posta bildirimi gönder
                await SendStokNotificationAsync(stok);

                // Kritik stok kontrolü ve bildirim
                if (stok.Adet < 10)
                {
                    var adminKullanicilar = _context.Kullanicilar.Where(k => k.Rol == "Admin" && k.IsActive).ToList();
                    foreach (var admin in adminKullanicilar)
                    {
                        BildirimController.BildirimOlustur(_context, admin.ID,
                            "Kritik Stok Uyarısı",
                            $"{stok.MalzemeAdi} stok seviyesi kritik: {stok.Adet} adet",
                            "warning",
                            "/Stok/Index");
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(stok);
        }

        public IActionResult Deactivate(int id)
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            var stok = _context.Stoklar.FirstOrDefault(s => s.ID == id);
            if (stok == null)
                return NotFound();

            var malzemeAdi = stok.MalzemeAdi;
            var adet = stok.Adet;
            stok.IsActive = false;
            _context.SaveChanges();

            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            LogController.LogEkle(_context, kullaniciAdi, $"Stok pasifleştirildi: {malzemeAdi} - Miktar: {adet}");

            return RedirectToAction(nameof(Index));
        }

        private async Task SendStokNotificationAsync(Stok stok)
        {
            try
            {
                var adminKullanicilar = _context.Kullanicilar
                    .Where(k => k.Rol == "Admin" && k.IsActive)
                    .Select(k => new { k.Email })
                    .ToList();
                
                foreach (var admin in adminKullanicilar)
                {
                    var subject = "Yeni Stok Eklendi";
                    var message = $"Yeni bir stok eklendi:\n\n" +
                                 $"Malzeme: {stok.MalzemeAdi}\n" +
                                 $"Miktar: {stok.Adet}\n" +
                                 $"Birim Fiyat: {stok.BirimFiyat} TL\n" +
                                 $"Giriş Tarihi: {stok.GirisTarihi:dd.MM.yyyy HH:mm}";

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
