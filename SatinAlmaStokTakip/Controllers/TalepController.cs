using Microsoft.AspNetCore.Mvc;
using SatinAlmaStokTakip.Models;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using SatinAlmaStokTakip.Services;

namespace SatinAlmaStokTakip.Controllers
{
    public class TalepController : Controller
    {
        private readonly VeritabaniContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailService _emailService;

        public TalepController(VeritabaniContext context, IWebHostEnvironment webHostEnvironment, IEmailService emailService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            var talepler = _context.Talepler.Where(t => t.IsActive).ToList();
            return View(talepler);
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Talep talep, IFormFile? dosya)
        {
            if (ModelState.IsValid)
            {
                talep.TalepTarihi = DateTime.Now;
                talep.Durum ??= "Beklemede";

                // Dosya yükleme işlemi
                if (dosya != null && dosya.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + dosya.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        dosya.CopyTo(fileStream);
                    }

                    talep.DosyaYolu = "/uploads/" + uniqueFileName;
                }

                _context.Talepler.Add(talep);
                _context.SaveChanges();

                var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
                LogController.LogEkle(_context, kullaniciAdi, $"Yeni talep oluşturuldu: ID {talep.ID}");

                // E-posta bildirimi gönder
                await SendTalepNotificationAsync(talep);

                // Bildirim oluştur
                var adminKullanicilar = _context.Kullanicilar.Where(k => k.Rol == "Admin" && k.IsActive).ToList();
                foreach (var admin in adminKullanicilar)
                {
                    BildirimController.BildirimOlustur(_context, admin.ID, 
                        "Yeni Talep Oluşturuldu", 
                        $"Talep ID: {talep.ID} oluşturuldu.", 
                        "info", 
                        $"/Talep/Detay/{talep.ID}");
                }

                return RedirectToAction(nameof(Index));
            }
            return View(talep);
        }

        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            var talep = _context.Talepler.FirstOrDefault(t => t.ID == id);
            if (talep == null)
                return NotFound();

            return View(talep);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Talep guncel, IFormFile? dosya)
        {
            if (ModelState.IsValid)
            {
                var talep = _context.Talepler.FirstOrDefault(t => t.ID == guncel.ID);
                if (talep == null)
                    return NotFound();

                var eskiDurum = talep.Durum;
                talep.Durum = guncel.Durum;

                // Dosya yükleme işlemi
                if (dosya != null && dosya.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + dosya.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        dosya.CopyTo(fileStream);
                    }

                    talep.DosyaYolu = "/uploads/" + uniqueFileName;
                }

                _context.SaveChanges();

                var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
                LogController.LogEkle(_context, kullaniciAdi, $"Talep durumu güncellendi: ID {talep.ID} - {eskiDurum} → {guncel.Durum}");

                // Durum değişikliği bildirimi gönder
                if (eskiDurum != guncel.Durum)
                {
                    await SendDurumDegisikligiNotificationAsync(talep, eskiDurum, guncel.Durum ?? "Bilinmiyor");
                }

                return RedirectToAction(nameof(Index));
            }
            return View(guncel);
        }

        public IActionResult Detay(int id)
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            var talep = _context.Talepler
                .Where(t => t.ID == id)
                .Select(t => new SatinAlmaStokTakip.Models.Talep {
                    ID = t.ID,
                    KullaniciID = t.KullaniciID,
                    TalepTarihi = t.TalepTarihi,
                    Durum = t.Durum,
                    OnayDurumu = t.OnayDurumu,
                    OnayTarihi = t.OnayTarihi,
                    OnaylayanKullaniciID = t.OnaylayanKullaniciID,
                    OnayNotu = t.OnayNotu,
                    DosyaYolu = t.DosyaYolu,
                    IsActive = t.IsActive,
                    Kullanici = t.Kullanici,
                    Teklifler = t.Teklifler,
                    Yorumlar = _context.Yorumlar
                        .Where(y => y.Tip == "Talep" && y.KayitID == t.ID && y.IsActive)
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
            if (talep == null)
                return NotFound();

            var teklifler = _context.Teklifler.Where(t => t.TalepID == id).ToList();
            ViewBag.Talep = talep;
            return View(teklifler);
        }

        public IActionResult Deactivate(int id)
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            var talep = _context.Talepler.FirstOrDefault(t => t.ID == id);
            if (talep == null)
                return NotFound();

            talep.IsActive = false;
            _context.SaveChanges();

            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            LogController.LogEkle(_context, kullaniciAdi, $"Talep pasifleştirildi: ID {id}");

            return RedirectToAction(nameof(Index));
        }

        public IActionResult DownloadFile(int id)
        {
            var talep = _context.Talepler.FirstOrDefault(t => t.ID == id);
            if (talep == null || string.IsNullOrEmpty(talep.DosyaYolu))
                return NotFound();

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, talep.DosyaYolu.TrimStart('/'));
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var fileName = Path.GetFileName(talep.DosyaYolu);
            var contentType = "application/octet-stream";

            return PhysicalFile(filePath, contentType, fileName);
        }

        public IActionResult ExportToExcel()
        {
            var talepler = _context.Talepler.ToList();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Talepler");
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Kullanıcı ID";
                worksheet.Cell(1, 3).Value = "Talep Tarihi";
                worksheet.Cell(1, 4).Value = "Durum";
                worksheet.Cell(1, 5).Value = "Dosya Yolu";

                int row = 2;
                foreach (var t in talepler)
                {
                    worksheet.Cell(row, 1).Value = t.ID;
                    worksheet.Cell(row, 2).Value = t.KullaniciID;
                    worksheet.Cell(row, 3).Value = t.TalepTarihi.ToString("yyyy-MM-dd HH:mm");
                    worksheet.Cell(row, 4).Value = t.Durum;
                    worksheet.Cell(row, 5).Value = t.DosyaYolu;
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Talepler.xlsx");
                }
            }
        }

        private async Task SendTalepNotificationAsync(Talep talep)
        {
            try
            {
                var adminKullanicilar = _context.Kullanicilar
                    .Where(k => k.Rol == "Admin" && k.IsActive)
                    .Select(k => new { k.Email })
                    .ToList();
                
                foreach (var admin in adminKullanicilar)
                {
                    var subject = "Yeni Talep Oluşturuldu";
                    var message = $"Yeni bir talep oluşturuldu:\n\n" +
                                 $"Talep ID: {talep.ID}\n" +
                                 $"Durum: {talep.Durum}\n" +
                                 $"Tarih: {talep.TalepTarihi:dd.MM.yyyy HH:mm}\n" +
                                 $"Dosya: {(string.IsNullOrEmpty(talep.DosyaYolu) ? "Yok" : "Var")}";

                    await _emailService.SendNotificationAsync(admin.Email ?? "admin@example.com", subject, message);
                }
            }
            catch (Exception ex)
            {
                // E-posta gönderme hatası loglanabilir
                Console.WriteLine($"E-posta gönderme hatası: {ex.Message}");
            }
        }

        private async Task SendDurumDegisikligiNotificationAsync(Talep talep, string eskiDurum, string yeniDurum)
        {
            try
            {
                var adminKullanicilar = _context.Kullanicilar
                    .Where(k => k.Rol == "Admin" && k.IsActive)
                    .Select(k => new { k.Email })
                    .ToList();
                
                foreach (var admin in adminKullanicilar)
                {
                    var subject = "Talep Durumu Güncellendi";
                    var message = $"Talep durumu güncellendi:\n\n" +
                                 $"Talep ID: {talep.ID}\n" +
                                 $"Eski Durum: {eskiDurum}\n" +
                                 $"Yeni Durum: {yeniDurum}\n" +
                                 $"Güncelleme Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}";

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
