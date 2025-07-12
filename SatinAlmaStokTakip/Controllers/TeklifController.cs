using Microsoft.AspNetCore.Mvc;
using SatinAlmaStokTakip.Models;
using System.Linq;
using SatinAlmaStokTakip.Services;

namespace SatinAlmaStokTakip.Controllers
{
    public class TeklifController : Controller
    {
        private readonly VeritabaniContext _context;
        private readonly IEmailService _emailService;

        public TeklifController(VeritabaniContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            var teklifler = _context.Teklifler.Where(t => t.IsActive).ToList();
            return View(teklifler);
        }

        public IActionResult Create(int talepID)
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            ViewBag.TalepID = talepID;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teklif teklif)
        {
            if (ModelState.IsValid)
            {
                _context.Teklifler.Add(teklif);
                _context.SaveChanges();

                var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
                LogController.LogEkle(_context, kullaniciAdi, $"Yeni teklif oluşturuldu: {teklif.FirmaAdi} - Fiyat: {teklif.Fiyat} TL");

                // E-posta bildirimi gönder
                await SendTeklifNotificationAsync(teklif);

                return RedirectToAction("Detay", "Talep", new { id = teklif.TalepID });
            }

            ViewBag.TalepID = teklif.TalepID;
            return View(teklif);
        }

        public IActionResult Deactivate(int id)
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            var teklif = _context.Teklifler.FirstOrDefault(t => t.ID == id);
            if (teklif == null)
                return NotFound();

            var firmaAdi = teklif.FirmaAdi;
            var fiyat = teklif.Fiyat;
            teklif.IsActive = false;
            _context.SaveChanges();

            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            LogController.LogEkle(_context, kullaniciAdi, $"Teklif pasifleştirildi: {firmaAdi} - Fiyat: {fiyat} TL");

            return RedirectToAction("Index");
        }

        public IActionResult Detay(int id)
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            var teklif = _context.Teklifler
                .Where(t => t.ID == id)
                .Select(t => new Teklif {
                    ID = t.ID,
                    TalepID = t.TalepID,
                    FirmaAdi = t.FirmaAdi,
                    Fiyat = t.Fiyat,
                    Notlar = t.Notlar,
                    OnayDurumu = t.OnayDurumu,
                    OnayTarihi = t.OnayTarihi,
                    OnaylayanKullaniciID = t.OnaylayanKullaniciID,
                    OnayNotu = t.OnayNotu,
                    DosyaYolu = t.DosyaYolu,
                    IsActive = t.IsActive,
                    Talep = t.Talep,
                    OnaylayanKullanici = t.OnaylayanKullanici,
                    Yorumlar = _context.Yorumlar
                        .Where(y => y.Tip == "Teklif" && y.KayitID == t.ID && y.IsActive)
                        .Select(y => new Yorum {
                            ID = y.ID,
                            Icerik = y.Icerik,
                            Tarih = y.Tarih,
                            KullaniciID = y.KullaniciID,
                            Tip = y.Tip,
                            KayitID = y.KayitID,
                            IsActive = y.IsActive,
                            Kullanici = y.Kullanici
                        }).ToList()
                }).FirstOrDefault();
            if (teklif == null)
                return NotFound();

            return View(teklif);
        }

        private async Task SendTeklifNotificationAsync(Teklif teklif)
        {
            try
            {
                var adminKullanicilar = _context.Kullanicilar
                    .Where(k => k.Rol == "Admin" && k.IsActive)
                    .Select(k => new { k.Email })
                    .ToList();
                
                foreach (var admin in adminKullanicilar)
                {
                    var subject = "Yeni Teklif Alındı";
                    var message = $"Yeni bir teklif alındı:\n\n" +
                                 $"Teklif ID: {teklif.ID}\n" +
                                 $"Firma: {teklif.FirmaAdi}\n" +
                                 $"Fiyat: {teklif.Fiyat} TL\n" +
                                 $"Talep ID: {teklif.TalepID}\n" +
                                 $"Tarih: {DateTime.Now:dd.MM.yyyy HH:mm}";

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
