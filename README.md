# 🏢 Satın Alma Stok Takip Sistemi

Modern ve kullanıcı dostu bir stok takip ve satın alma yönetim sistemi. Tailwind CSS ile tasarlanmış responsive arayüz, MySQL veritabanı ve .NET 8 teknolojileri kullanılarak geliştirilmiştir.

## ✨ Özellikler

### 📊 Dashboard & Analitik
- **Gerçek zamanlı istatistikler** - Toplam talep, teklif, fatura ve stok sayıları
- **Grafiksel raporlar** - Chart.js ile interaktif grafikler
- **Stok durumu takibi** - Kritik seviyedeki ürünler için uyarılar
- **Son işlemler** - Sistemdeki son aktivitelerin özeti

### 🛒 Talep Yönetimi
- **Talep oluşturma** - Detaylı talep formları
- **Onay süreci** - Çok aşamalı onay mekanizması
- **Dosya yükleme** - Talep belgelerinin sisteme eklenmesi
- **Durum takibi** - Taleplerin anlık durumları

### 💼 Teklif Yönetimi
- **Teklif karşılaştırma** - Farklı firmaların tekliflerini karşılaştırma
- **Onay sistemi** - Teklif onay/red mekanizması
- **Dosya yönetimi** - Teklif belgelerinin saklanması

### 📋 Stok Yönetimi
- **Stok girişi** - Yeni ürünlerin stoka eklenmesi
- **Stok takibi** - Mevcut stok seviyelerinin izlenmesi
- **Kritik seviye uyarıları** - Düşük stok uyarıları
- **Tüketim kayıtları** - Stok çıkışlarının takibi

### 🧾 Fatura Yönetimi
- **Fatura oluşturma** - Teklif bazlı fatura oluşturma
- **Fatura takibi** - Fatura durumlarının izlenmesi
- **Tutar hesaplamaları** - Otomatik tutar hesaplamaları

### 👥 Kullanıcı Yönetimi
- **Rol tabanlı erişim** - Admin, Yetkili, Personel rolleri
- **Kullanıcı profilleri** - Detaylı kullanıcı bilgileri
- **Güvenli giriş** - Session tabanlı kimlik doğrulama

### 📧 Bildirim Sistemi
- **E-posta bildirimleri** - Otomatik e-posta gönderimi
- **Sistem bildirimleri** - Anlık sistem uyarıları

### 📈 Raporlama
- **Excel export** - Verilerin Excel formatında dışa aktarılması
- **Tarih bazlı raporlar** - Belirli tarih aralıklarında raporlar
- **Grafiksel analizler** - Görsel raporlama araçları

## 🛠️ Teknolojiler

- **Backend**: .NET 8, ASP.NET Core MVC
- **Veritabanı**: MySQL 8.0
- **ORM**: Entity Framework Core 8
- **Frontend**: HTML5, CSS3, JavaScript
- **CSS Framework**: Tailwind CSS
- **Grafikler**: Chart.js
- **E-posta**: MailKit
- **Excel**: ClosedXML

## 📋 Sistem Gereksinimleri

- **.NET 8 SDK** veya üzeri
- **MySQL 8.0** veya üzeri
- **Visual Studio 2022** veya **VS Code**
- **Git** (versiyon kontrolü için)

## 🚀 Kurulum Adımları

### 1. Projeyi İndirin
```bash
git clone https://github.com/kullaniciadi/SatinAlmaStokTakip.git
cd SatinAlmaStokTakip
```

### 2. Veritabanını Hazırlayın
```sql
-- MySQL'de yeni veritabanı oluşturun
CREATE DATABASE stokdb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

### 3. Bağlantı Ayarlarını Yapın
`SatinAlmaStokTakip/appsettings.json` dosyasındaki veritabanı bağlantı bilgilerini güncelleyin:

```json
{
  "ConnectionStrings": {
    "mysqlbaglanti": "server=localhost;port=3306;database=stokdb;user=root;password=sifreniz"
  }
}
```

### 4. E-posta Ayarlarını Yapın
`appsettings.json` dosyasında e-posta ayarlarını güncelleyin:

```json
{
  "Email": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "Username": "emailiniz@gmail.com",
    "Password": "uygulama_sifresi",
    "FromEmail": "emailiniz@gmail.com",
    "FromName": "Satın Alma Stok Takip Sistemi"
  }
}
```

### 5. Bağımlılıkları Yükleyin
```bash
dotnet restore
```

### 6. Veritabanını Oluşturun
```bash
dotnet ef database update
```

### 7. Uygulamayı Çalıştırın
```bash
dotnet run
```

### 8. Tarayıcıda Açın
```
https://localhost:7001
```

## 👤 Varsayılan Kullanıcı Bilgileri

Sistem ilk çalıştırıldığında otomatik olarak bir admin kullanıcısı oluşturulur:

- **Kullanıcı Adı**: `admin`
- **Şifre**: `123456`
- **Rol**: `Admin`
- **E-posta**: `admin@example.com`

## 📱 Kullanım Kılavuzu

### Dashboard
- **Ana sayfa** - Sistem genel durumu ve istatistikler
- **Hızlı işlemler** - Sık kullanılan işlemlere hızlı erişim
- **Grafikler** - Stok ve fatura verilerinin görsel analizi

### Talep Yönetimi
1. **Yeni Talep Oluşturma**
   - "Talep" menüsünden "Yeni Talep" seçin
   - Gerekli bilgileri doldurun
   - Dosya ekleyin (isteğe bağlı)
   - "Kaydet" butonuna tıklayın

2. **Talep Onaylama**
   - Admin/Yetkili kullanıcılar talep listesini görüntüler
   - "Detay" butonuna tıklayarak talep detaylarını inceleyin
   - "Onayla" veya "Reddet" seçeneklerini kullanın

### Teklif Yönetimi
1. **Teklif Oluşturma**
   - "Teklif" menüsünden "Yeni Teklif" seçin
   - İlgili talebi seçin
   - Firma bilgilerini ve fiyatı girin
   - "Kaydet" butonuna tıklayın

2. **Teklif Karşılaştırma**
   - Teklif listesinde farklı firmaların tekliflerini görüntüleyin
   - "Detay" butonuna tıklayarak detaylı karşılaştırma yapın

### Stok Yönetimi
1. **Stok Girişi**
   - "Stok" menüsünden "Yeni Stok" seçin
   - Malzeme adı, adet ve birim fiyat bilgilerini girin
   - "Kaydet" butonuna tıklayın

2. **Stok Takibi**
   - Stok listesinde mevcut stokları görüntüleyin
   - Kritik seviyedeki ürünler kırmızı ile işaretlenir
   - "Düzenle" butonu ile stok bilgilerini güncelleyin

### Fatura Yönetimi
1. **Fatura Oluşturma**
   - "Fatura" menüsünden "Yeni Fatura" seçin
   - İlgili teklifi seçin
   - Fatura bilgilerini doldurun
   - "Kaydet" butonuna tıklayın

### Kullanıcı Yönetimi (Admin)
1. **Yeni Kullanıcı Ekleme**
   - "Kullanıcı" menüsünden "Yeni Kullanıcı" seçin
   - Kullanıcı bilgilerini doldurun
   - Rol ataması yapın
   - "Kaydet" butonuna tıklayın

## 🔧 Geliştirme

### Proje Yapısı
```
SatinAlmaStokTakip/
├── Controllers/          # MVC Controller'ları
├── Models/              # Veritabanı modelleri
├── Views/               # Razor view'ları
├── Migrations/          # Entity Framework migrations
├── wwwroot/            # Statik dosyalar
└── Program.cs          # Uygulama başlangıç noktası
```

### Yeni Migration Oluşturma
```bash
dotnet ef migrations add MigrationAdi
dotnet ef database update
```

### Yeni Controller Ekleme
```bash
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet tool install -g dotnet-aspnet-codegenerator
dotnet aspnet-codegenerator controller -m ModelAdi -dc VeritabaniContext -outDir Controllers
```

## 🐛 Sorun Giderme

### Veritabanı Bağlantı Hatası
- MySQL servisinin çalıştığından emin olun
- Bağlantı dizesindeki bilgileri kontrol edin
- Firewall ayarlarını kontrol edin

### E-posta Gönderim Hatası
- SMTP ayarlarını kontrol edin
- Gmail kullanıyorsanız "Uygulama Şifresi" oluşturun
- Port numarasının doğru olduğundan emin olun

### Uygulama Çalışmıyor
- .NET 8 SDK'nın yüklü olduğundan emin olun
- `dotnet restore` komutunu çalıştırın
- Port çakışması varsa `launchSettings.json` dosyasını düzenleyin

## 📄 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için `LICENSE` dosyasına bakın.

## 🤝 Katkıda Bulunma

1. Bu repository'yi fork edin
2. Yeni bir branch oluşturun (`git checkout -b feature/yeni-ozellik`)
3. Değişikliklerinizi commit edin (`git commit -am 'Yeni özellik eklendi'`)
4. Branch'inizi push edin (`git push origin feature/yeni-ozellik`)
5. Pull Request oluşturun

## 📞 İletişim

- **Geliştirici**: er0s3c
- **E-posta**: erenaloglu@yaani.com
- **GitHub**: https://github.com/er0s3c

