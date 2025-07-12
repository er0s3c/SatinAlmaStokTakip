using Microsoft.AspNetCore.Mvc;
using SatinAlmaStokTakip.Models;
using System.Linq;
using SatinAlmaStokTakip.Services;

namespace SatinAlmaStokTakip.Controllers
{
    public class FaturaController : Controller
    {
        private readonly VeritabaniContext _context;
        private readonly IEmailService _emailService;

        public FaturaController(VeritabaniContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            var faturalar = _context.Faturalar.Where(f => f.IsActive).ToList();
            return View(faturalar);
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            ViewBag.Teklifler = _context.Teklifler.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Fatura fatura)
        {
            if (ModelState.IsValid)
            {
                fatura.FaturaTarihi = DateTime.Now;
                _context.Faturalar.Add(fatura);
                _context.SaveChanges();

                var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
                LogController.LogEkle(_context, kullaniciAdi, $"Yeni fatura oluşturuldu: {fatura.FaturaNo} - Tutar: {fatura.Tutar} TL");

                // E-posta bildirimi gönder
                await SendFaturaNotificationAsync(fatura);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Teklifler = _context.Teklifler.ToList();
            return View(fatura);
        }

        public IActionResult Deactivate(int id)
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            var fatura = _context.Faturalar.FirstOrDefault(f => f.ID == id);
            if (fatura == null)
                return NotFound();

            var faturaNo = fatura.FaturaNo;
            var tutar = fatura.Tutar;
            fatura.IsActive = false;
            _context.SaveChanges();

            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            LogController.LogEkle(_context, kullaniciAdi, $"Fatura pasifleştirildi: {faturaNo} - Tutar: {tutar} TL");

            return RedirectToAction(nameof(Index));
        }

        private async Task SendFaturaNotificationAsync(Fatura fatura)
        {
            try
            {
                var adminKullanicilar = _context.Kullanicilar
                    .Where(k => k.Rol == "Admin" && k.IsActive)
                    .Select(k => new { k.Email })
                    .ToList();
                
                foreach (var admin in adminKullanicilar)
                {
                    var subject = "Yeni Fatura Oluşturuldu";
                    var message = $"Yeni bir fatura oluşturuldu:\n\n" +
                                 $"Fatura No: {fatura.FaturaNo}\n" +
                                 $"Tutar: {fatura.Tutar} TL\n" +
                                 $"Tarih: {fatura.FaturaTarihi:dd.MM.yyyy HH:mm}\n" +
                                 $"Teklif ID: {fatura.TeklifID}";

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
