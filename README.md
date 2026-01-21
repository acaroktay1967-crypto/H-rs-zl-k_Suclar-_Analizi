# Metin Editörü - WPF Tabanlı Gelişmiş Doküman İşleme Uygulaması

## Proje Hakkında

Bu proje, Türkiye Adalet Sistemi ve genel ofis kullanımı için geliştirilmiş, WPF tabanlı gelişmiş bir metin editörüdür. Unicode karakter vurgulama, çeşitli format destekleri ve profesyonel doküman işleme özellikleriyle donatılmıştır.

## Özellikler

### 1. Unicode İşaretlerin Renklendirilmesi
- Özel karakterlerin ve Unicode işaretlerinin otomatik tespiti
- Özel karakterlerin kırmızı renkte vurgulanması
- Gerçek zamanlı karakter analizi
- Geniş Unicode karakter aralığı desteği (Latin Extended, Greek, Cyrillic, Arabic, Hebrew, Mathematical Operators, vb.)

### 2. Metin Düzenleme Araçları
- **Temel İşlemler:**
  - Kes (Ctrl+X)
  - Kopyala (Ctrl+C)
  - Yapıştır (Ctrl+V)
  - Tümünü Seç (Ctrl+A)

- **Biçimlendirme:**
  - Kalın (Bold - Ctrl+B)
  - İtalik (Italic - Ctrl+I)
  - Altı Çizili (Underline - Ctrl+U)

- **Yazı Tipi Seçenekleri:**
  - Sistem yazı tiplerinden seçim
  - Özelleştirilebilir yazı boyutu (8-72 punto arası)

### 3. Çıktı Alma Formatları

#### Word (.docx)
- OpenXML SDK kullanılarak profesyonel Word belgesi oluşturma
- Paragraf ve metin yapısının korunması
- Microsoft Word ile tam uyumluluk

#### PDF
- iTextSharp kütüphanesi ile PDF çıktısı
- Unicode karakter desteği
- Türkçe karakter tam desteği
- A4 sayfa formatı

#### UYAP UDF
- Türkiye Adalet Sistemi'nin UYAP formatı
- XML tabanlı standart format
- Tarih damgası ve yapılandırılmış içerik desteği
- Adli belgeler için optimize edilmiş

### 4. Yazdırma Özelliği
- Windows yazdırma sistemi entegrasyonu
- Yazıcı seçimi ve ayarları
- Önizleme desteği
- Tüm biçimlendirmelerin korunması

### 5. Kullanıcı Arayüzü
- Modern ve sezgisel WPF arayüzü
- Menü çubuğu ile kolay erişim
- Araç çubuğu ile hızlı işlem erişimi
- Durum çubuğu ile karakter sayısı takibi
- Gerçek zamanlı geri bildirim

## Teknik Detaylar

### Kullanılan Teknolojiler
- **.NET 8.0** - Modern ve performanslı framework
- **WPF (Windows Presentation Foundation)** - Zengin masaüstü uygulaması geliştirme
- **DocumentFormat.OpenXml** (v2.20.0) - Word belgesi oluşturma
- **iTextSharp.LGPLv2.Core** (v3.4.5) - PDF oluşturma

### Sistem Gereksinimleri
- Windows 10 veya üzeri
- .NET 8.0 Runtime
- Minimum 2 GB RAM
- 50 MB disk alanı

## Kurulum

### Geliştirme Ortamı
1. .NET 8.0 SDK'yı yükleyin
2. Projeyi klonlayın:
   ```bash
   git clone https://github.com/acaroktay1967-crypto/H-rs-zl-k_Suclar-_Analizi.git
   ```
3. Proje dizinine gidin:
   ```bash
   cd H-rs-zl-k_Suclar-_Analizi
   ```
4. Bağımlılıkları yükleyin:
   ```bash
   dotnet restore
   ```
5. Projeyi derleyin:
   ```bash
   dotnet build
   ```
6. Uygulamayı çalıştırın:
   ```bash
   dotnet run
   ```

## Kullanım

### Yeni Belge Oluşturma
1. Menüden **Dosya > Yeni** seçeneğini tıklayın
2. Yeni bir boş belge açılacaktır

### Metin Biçimlendirme
1. Biçimlendirmek istediğiniz metni seçin
2. Araç çubuğundan veya menüden istediğiniz biçimi seçin
3. Yazı tipi ve boyutunu değiştirmek için açılır menüleri kullanın

### Doküman Dışa Aktarma
1. **Dosya** menüsünden istediğiniz format seçeneğini seçin:
   - Word'e Aktar (.docx)
   - PDF'e Aktar (.pdf)
   - UYAP UDF'e Aktar (.udf)
2. Kayıt konumunu ve dosya adını belirleyin
3. **Kaydet** butonuna tıklayın

### Yazdırma
1. **Dosya > Yazdır** veya araç çubuğundaki **Yazdır** butonunu tıklayın
2. Yazıcı ayarlarını yapın
3. **Yazdır** butonuna tıklayın

## Unicode Karakter Vurgulama

Uygulama aşağıdaki Unicode karakter aralıklarını otomatik olarak tespit eder ve vurgular:
- Latin Extended (U+0080 - U+024F)
- Greek (U+0370 - U+03FF)
- Cyrillic (U+0400 - U+04FF)
- Armenian (U+0530 - U+058F)
- Hebrew (U+0590 - U+05FF)
- Arabic (U+0600 - U+06FF)
- General Punctuation (U+2000 - U+206F)
- Mathematical Operators (U+2200 - U+22FF)
- Arrows (U+2190 - U+21FF)
- Box Drawing (U+2500 - U+257F)
- Geometric Shapes (U+25A0 - U+25FF)
- Ve daha fazlası...

## Proje Yapısı

```
H-rs-zl-k_Suclar-_Analizi/
├── App.xaml                 # Uygulama giriş noktası
├── App.xaml.cs             # Uygulama kod arkası
├── MainWindow.xaml         # Ana pencere arayüzü
├── MainWindow.xaml.cs      # Ana pencere mantığı
├── TextEditor.csproj       # Proje yapılandırması
├── .gitignore              # Git göz ardı dosyaları
└── README.md               # Proje dokümantasyonu
```

## Geliştirme Planı

### Tamamlanan Özellikler
- ✅ WPF proje yapısı
- ✅ Ana pencere ve kullanıcı arayüzü
- ✅ Unicode karakter vurgulama
- ✅ Temel metin düzenleme araçları
- ✅ Metin biçimlendirme (Bold, Italic, Underline)
- ✅ Yazı tipi ve boyut seçimi
- ✅ Word (.docx) dışa aktarma
- ✅ PDF dışa aktarma
- ✅ UYAP UDF dışa aktarma
- ✅ Yazdırma özelliği
- ✅ Dosya açma/kaydetme (RTF)

### Gelecek Geliştirmeler
- 🔄 Ribbon UI entegrasyonu
- 🔄 Geri al/İleri al (Undo/Redo) özelliği
- 🔄 Bul ve değiştir işlevselliği
- 🔄 Otomatik kaydetme
- 🔄 Çoklu belge desteği (tabbed interface)
- 🔄 Yazım denetimi
- 🔄 Karanlık tema desteği

## Katkıda Bulunma

Katkılarınızı bekliyoruz! Lütfen şu adımları izleyin:
1. Projeyi fork edin
2. Özellik dalınızı oluşturun (`git checkout -b feature/YeniOzellik`)
3. Değişikliklerinizi commit edin (`git commit -m 'Yeni özellik eklendi'`)
4. Dalınıza push yapın (`git push origin feature/YeniOzellik`)
5. Pull Request oluşturun

## Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakın.

## İletişim

Proje Sahibi: [acaroktay1967-crypto](https://github.com/acaroktay1967-crypto)

Proje Linki: [https://github.com/acaroktay1967-crypto/H-rs-zl-k_Suclar-_Analizi](https://github.com/acaroktay1967-crypto/H-rs-zl-k_Suclar-_Analizi)

## Teşekkürler

Bu projeyi geliştirirken kullandığımız açık kaynak kütüphanelerin geliştiricilerine teşekkür ederiz:
- DocumentFormat.OpenXml ekibine
- iTextSharp geliştiricilerine
- .NET ve WPF ekiplerine

---

**Not:** Bu uygulama Türkiye Adalet Sistemi için UYAP formatı desteği ile geliştirilmiştir ve adli belge işleme ihtiyaçlarını karşılamak üzere tasarlanmıştır.
