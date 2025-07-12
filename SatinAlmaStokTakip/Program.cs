using Microsoft.EntityFrameworkCore;
using SatinAlmaStokTakip.Models;
using SatinAlmaStokTakip.Services;

var builder = WebApplication.CreateBuilder(args);

// Veritabanı bağlantı dizesi (appsettings.json içinden)
var connectionString = builder.Configuration.GetConnectionString("mysqlbaglanti");

// MySQL + Entity Framework Core 8 (Pomelo)
builder.Services.AddDbContext<VeritabaniContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// E-posta servisi
builder.Services.AddScoped<IEmailService, EmailService>();

// MVC ve Razor view servisi
builder.Services.AddControllersWithViews();

// Session ve HTTP Context desteği
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Veritabanını oluştur ve seed data ekle
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<VeritabaniContext>();
    context.Database.EnsureCreated();
    
    // Örnek admin kullanıcısı ekle (eğer yoksa)
    if (!context.Kullanicilar.Any())
    {
        var adminKullanici = new Kullanici
        {
            AdSoyad = "Sistem Yöneticisi",
            KullaniciAdi = "admin",
            Sifre = "123456",
            Rol = "Admin",
            Email = "admin@example.com"
        };
        
        context.Kullanicilar.Add(adminKullanici);
        context.SaveChanges();
        
        Console.WriteLine("Örnek admin kullanıcısı oluşturuldu:");
        Console.WriteLine($"Kullanıcı Adı: admin");
        Console.WriteLine($"Şifre: 123456");
        Console.WriteLine($"Rol: Admin");
        Console.WriteLine($"E-posta: admin@example.com");
    }
}

// Hata ayarları ve HTTPS yönlendirmesi
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Session aktif et
app.UseSession();

// Giriş kontrolü için (isteğe bağlı, kullanıyorsan)
app.UseAuthorization();

// Varsayılan rotalama ayarı (HomeController / Index)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
