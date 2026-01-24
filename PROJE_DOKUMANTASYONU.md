# Proje Dokümantasyonu

## Genel Bakış

Bu proje, Türk Ceza Kanunu'nun 141 ve 142. maddelerine göre hırsızlık suçlarının analizi ve ceza hesaplamalarını yapan modern bir WPF uygulamasıdır.

## Mimari ve Yapı

### Katmanlı Mimari

```
HirsizlikSuclariAnalizi/
├── Models/              # Veri modelleri
│   ├── HirsizlikTuru.cs
│   ├── CezaHesaplama.cs
│   └── YargitayKarari.cs
├── Services/            # İş mantığı servisleri
│   ├── CezaHesaplamaService.cs
│   └── PdfService.cs
├── Data/                # Veritabanı işlemleri
│   └── DatabaseService.cs
├── Views/               # Kullanıcı arayüzü görünümleri
│   ├── CezaHesaplamaView.xaml
│   ├── CezaHesaplamaView.xaml.cs
│   ├── YargitayKararlariView.xaml
│   └── YargitayKararlariView.xaml.cs
└── MainWindow.xaml      # Ana pencere
```

## Önemli Sınıflar

### Models

#### HirsizlikTuru (Enum)
```csharp
public enum HirsizlikTuru
{
    BasitHirsizlik,      // TCK 141
    NitelikliHirsizlik   // TCK 142
}
```

#### CezaHesaplama
Ceza hesaplama bilgilerini tutan model:
- HirsizlikTuru: Suç türü
- MalDegeri: Çalınan malın değeri (TL)
- GeceVakti: Gece işlenip işlenmediği
- EtkinPismanlik: Etkin pişmanlık olup olmadığı
- MinCeza/MaxCeza: Hesaplanan ceza aralığı (yıl)
- HesaplamaTarihi: Hesaplama zamanı
- Notlar: Ek notlar

#### YargitayKarari
Yargıtay kararı bilgilerini tutan model:
- KararNumarasi: Karar numarası (örn: "2019/1234")
- SucTuru: Suç türü
- KararTarihi: Karar tarihi
- Ozet: Kararın özeti
- TamMetin: Kararın tam metni

### Services

#### CezaHesaplamaService
Ceza hesaplama iş mantığını içerir.

**Temel Sabitler:**
- Basit Hırsızlık: 1-3 yıl
- Nitelikli Hırsızlık Artırım: x1.5
- Etkin Pişmanlık İndirim: x0.67 (yaklaşık %33)

**Metotlar:**
- `HesaplaCeza()`: Ceza hesaplama algoritması
- `GetCezaAciklamasi()`: Yasal dayanak açıklaması

#### PdfService
QuestPDF kullanarak PDF rapor üretimi.

**Metotlar:**
- `GenerateCezaRaporu()`: Ceza hesaplama raporu
- `GenerateYargitayKarariRaporu()`: Yargıtay karar raporu

### Data

#### DatabaseService
SQLite veritabanı işlemleri.

**Özellikler:**
- Otomatik veritabanı oluşturma
- Örnek veri yükleme
- CRUD işlemleri
- Filtreleme ve arama

**Metotlar:**
- `InitializeDatabase()`: Veritabanı başlatma
- `SaveCezaHesaplama()`: Ceza hesaplama kaydetme
- `GetYargitayKararlari()`: Yargıtay kararlarını getirme
- `GetCezaHesaplamalari()`: Ceza hesaplamalarını getirme

## Ceza Hesaplama Algoritması

### Adım 1: Temel Ceza Belirleme

```
Basit Hırsızlık:
  MinCeza = 1 yıl
  MaxCeza = 3 yıl

Nitelikli Hırsızlık:
  MinCeza = 1.5 yıl
  MaxCeza = 4.5 yıl
```

### Adım 2: Mal Değerine Göre Ayarlama

```
Düşük Değer (< 1.000 TL):
  MaxCeza = MinCeza + (MaxCeza - MinCeza) * 0.3

Yüksek Değer (> 10.000 TL):
  MinCeza = MinCeza + (MaxCeza - MinCeza) * 0.6
```

### Adım 3: Gece Vakti Artırımı

```
Eğer geceVakti ve basitHırsızlık:
  MinCeza *= 1.5
  MaxCeza *= 1.5
```

### Adım 4: Etkin Pişmanlık İndirimi

```
Eğer etkinPismanlik:
  MinCeza *= 0.67
  MaxCeza *= 0.67
```

## Veritabanı Şeması

### CezaHesaplama Tablosu
```sql
CREATE TABLE CezaHesaplama (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    HirsizlikTuru INTEGER NOT NULL,
    MalDegeri REAL NOT NULL,
    GeceVakti INTEGER NOT NULL,
    EtkinPismanlik INTEGER NOT NULL,
    MinCeza REAL NOT NULL,
    MaxCeza REAL NOT NULL,
    HesaplamaTarihi TEXT NOT NULL,
    Notlar TEXT
);
```

### YargitayKarari Tablosu
```sql
CREATE TABLE YargitayKarari (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    KararNumarasi TEXT NOT NULL,
    SucTuru INTEGER NOT NULL,
    KararTarihi TEXT NOT NULL,
    Ozet TEXT NOT NULL,
    TamMetin TEXT NOT NULL
);
```

## Teknoloji Seçimleri

### .NET 8.0 WPF
- Modern Windows uygulama geliştirme
- Zengin UI bileşenleri
- XAML ile tasarım

### SQLite
- Hafif ve dosya tabanlı veritabanı
- Kurulum gerektirmez
- .NET entegrasyonu kolay

### QuestPDF
- Profesyonel PDF oluşturma
- Kod tabanlı PDF tasarımı
- Ücretsiz topluluk lisansı

## Güvenlik

### CodeQL Analizi
✅ Kod güvenlik açıkları tarandı - sorun bulunamadı

### Bağımlılık Güvenliği
✅ Tüm NuGet paketleri güvenlik açıkları için tarandı
- Microsoft.Data.Sqlite 8.0.0 - güvenli
- QuestPDF 2024.7.3 - güvenli

### En İyi Güvenlik Uygulamaları
- Parametreli SQL sorguları (SQL injection koruması)
- Input validasyonu
- Exception handling
- Güvenli dosya işlemleri

## Performans Optimizasyonları

- Veritabanı bağlantıları using bloklarında
- Lazy loading pattern
- Efficient LINQ queries
- UI responsiveness için async/await pattern kullanılabilir

## Gelecek Geliştirmeler

### Önerilen Özellikler
1. Kullanıcı yönetimi ve kimlik doğrulama
2. Raporlama modülü genişletme
3. Excel export özelliği
4. Grafik ve istatistikler
5. Yargıtay kararı ekleme/düzenleme UI
6. Zincirleme suç hesaplamaları
7. Çoklu dil desteği
8. Cloud veritabanı entegrasyonu

### Teknik İyileştirmeler
1. MVVM pattern implementasyonu
2. Dependency Injection
3. Unit testler
4. Integration testler
5. Logging mekanizması
6. Konfigürasyon yönetimi

## Test Senaryoları

### Manuel Test Senaryoları

#### Senaryo 1: Basit Hırsızlık Hesaplama
1. Uygulamayı başlat
2. "Ceza Hesaplama" modülünü aç
3. "Basit Hırsızlık" seç
4. Mal değeri: 500 TL
5. Ceza hesapla
6. Beklenen: 1 - 1.9 yıl hapis

#### Senaryo 2: Nitelikli Hırsızlık + Etkin Pişmanlık
1. "Nitelikli Hırsızlık" seç
2. Mal değeri: 15000 TL
3. "Etkin Pişmanlık" işaretle
4. Ceza hesapla
5. Beklenen: Yaklaşık 2 - 3 yıl hapis

#### Senaryo 3: Yargıtay Kararı Filtreleme
1. "Yargıtay Kararları" modülünü aç
2. Karar numarası: "2019"
3. Filtrele
4. Beklenen: 2019/1234 kararı gösterilmeli

#### Senaryo 4: PDF Oluşturma
1. Bir ceza hesapla
2. "PDF Oluştur" butonuna tıkla
3. Dosya konumu seç
4. Beklenen: PDF başarıyla oluşturulmalı

## Sorun Giderme

### Uygulama Başlamıyor
- .NET 8.0 SDK yüklü olduğundan emin olun
- Windows işletim sistemi kullanıldığından emin olun
- Proje klasöründe `dotnet restore` komutunu çalıştırın

### Veritabanı Hataları
- Uygulama klasöründe yazma izinleri olduğundan emin olun
- `hirsizlik.db` dosyasını silin ve uygulamayı yeniden başlatın

### PDF Oluşturma Hataları
- Yazma izinleri kontrol edin
- Disk alanı yeterli olduğundan emin olun
- PDF görüntüleyici kurulu olduğundan emin olun

## Lisans

MIT License - Detaylar için LICENSE dosyasına bakın.

## Katkıda Bulunma

1. Projeyi fork edin
2. Feature branch oluşturun
3. Değişikliklerinizi commit edin
4. Branch'inizi push edin
5. Pull Request açın

## İletişim

Proje GitHub sayfası: [H-rs-zl-k_Suclar-_Analizi](https://github.com/acaroktay1967-crypto/H-rs-zl-k_Suclar-_Analizi)
