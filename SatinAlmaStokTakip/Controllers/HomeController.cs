using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SatinAlmaStokTakip.Models;

namespace SatinAlmaStokTakip.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VeritabaniContext _context;

        public HomeController(ILogger<HomeController> logger, VeritabaniContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");

            // Dashboard verileri
            var dashboardData = new DashboardViewModel
            {
                ToplamTalepSayisi = _context.Talepler.Count(),
                BekleyenTalepSayisi = _context.Talepler.Count(t => t.Durum == "Beklemede"),
                ToplamTeklifSayisi = _context.Teklifler.Count(),
                ToplamFaturaSayisi = _context.Faturalar.Count(),
                ToplamStokSayisi = _context.Stoklar.Count(),
                DusukStokSayisi = _context.Stoklar.Count(s => s.Adet < 10),
                ToplamTuketimSayisi = _context.Tuketimler.Count(),
                SonTuketimTarihi = _context.Tuketimler.OrderByDescending(t => t.Tarih).FirstOrDefault()?.Tarih,
                SonTalepTarihi = _context.Talepler.OrderByDescending(t => t.TalepTarihi).FirstOrDefault()?.TalepTarihi,
                SonFaturaTarihi = _context.Faturalar.OrderByDescending(f => f.FaturaTarihi).FirstOrDefault()?.FaturaTarihi
            };

            return View(dashboardData);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Calendar()
        {
            if (HttpContext.Session.GetString("KullaniciAdi") == null)
                return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpGet]
        public JsonResult GetCalendarEvents()
        {
            var talepEvents = _context.Talepler.Select(t => new {
                id = "talep-" + t.ID,
                title = "Talep: " + t.ID,
                start = t.TalepTarihi.ToString("yyyy-MM-dd"),
                color = "#007bff",
                url = Url.Action("Detay", "Talep", new { id = t.ID })
            }).ToList();
            
            var faturaEvents = _context.Faturalar.Select(f => new {
                id = "fatura-" + f.ID,
                title = "Fatura: " + (f.FaturaNo ?? f.ID.ToString()),
                start = f.FaturaTarihi.ToString("yyyy-MM-dd"),
                color = "#28a745",
                url = Url.Action("Index", "Fatura")
            }).ToList();
            
            var events = talepEvents.Concat(faturaEvents).ToList();
            return Json(events);
        }

        [HttpGet]
        public JsonResult GetChartData()
        {
            // En çok talep edilen malzemeler (stok bazında)
            var stokVerileri = _context.Stoklar
                .Where(s => s.IsActive)
                .GroupBy(s => s.MalzemeAdi)
                .Select(g => new { Malzeme = g.Key, ToplamAdet = g.Sum(s => s.Adet) })
                .OrderByDescending(x => x.ToplamAdet)
                .Take(10)
                .ToList();

            // Aylık fatura tutarları - client-side formatting
            var aylikFaturaVerileri = _context.Faturalar
                .Where(f => f.IsActive && f.FaturaTarihi >= DateTime.Now.AddMonths(-6))
                .GroupBy(f => new { f.FaturaTarihi.Year, f.FaturaTarihi.Month })
                .Select(g => new { 
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    ToplamTutar = g.Sum(f => f.Tutar) 
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToList()
                .Select(x => new { 
                    Ay = $"{x.Year}-{x.Month:00}", 
                    ToplamTutar = x.ToplamTutar 
                })
                .ToList();

            // Stok durumları (kritik, normal, yüksek)
            var stokDurumlari = _context.Stoklar
                .Where(s => s.IsActive)
                .GroupBy(s => s.Adet < 10 ? "Kritik" : s.Adet < 50 ? "Normal" : "Yüksek")
                .Select(g => new { Durum = g.Key, Sayi = g.Count() })
                .ToList();

            return Json(new { 
                stokVerileri, 
                aylikFaturaVerileri, 
                stokDurumlari 
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class DashboardViewModel
    {
        public int ToplamTalepSayisi { get; set; }
        public int BekleyenTalepSayisi { get; set; }
        public int ToplamTeklifSayisi { get; set; }
        public int ToplamFaturaSayisi { get; set; }
        public int ToplamStokSayisi { get; set; }
        public int DusukStokSayisi { get; set; }
        public int ToplamTuketimSayisi { get; set; }
        public DateTime? SonTuketimTarihi { get; set; }
        public DateTime? SonTalepTarihi { get; set; }
        public DateTime? SonFaturaTarihi { get; set; }
    }
}
