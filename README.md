# Hırsızlık Suçları Analizi - WPF Uygulaması

Türk Ceza Kanunu'nun 141 ve 142. maddelerine uygun şekilde hırsızlık suçları analizi ve ceza hesaplama için geliştirilmiş WPF tabanlı bir masaüstü uygulamasıdır.

## 🎯 Özellikler

### 1. Ceza Hesaplama Modülü
- **Basit Hırsızlık (TCK 141)**: 1 yıldan 3 yıla kadar hapis cezası hesaplama
- **Nitelikli Hırsızlık (TCK 142)**: Nitelikli hallere göre ceza artırımları
  - TCK 142/1: 1/2 oranında artırım (gece vakti, birlikte suç, savunmasız mağdur, vb.)
  - TCK 142/2: 1/3 oranında artırım (mesken, işyeri, dini yerler, vb.)
- **İndirim Sebepleri**:
  - Etkin pişmanlık (TCK 168)
  - Teşebbüs hali (TCK 35)
  - Uzlaştırma (CMK 253)
- **Özel Durumlar**:
  - Akrabalar arası hırsızlık (TCK 167)
  - Hükmün açıklanmasının geri bırakılması (HAGB)

### 2. Yargıtay Kararları Modülü
- Örnek Yargıtay kararlarının veritabanında saklanması
- Anahtar kelime, TCK maddesi veya karar özeti ile arama
- Karar detaylarını görüntüleme
- İçtihat bilgilerine hızlı erişim

### 3. PDF Rapor Oluşturma
- Hesaplanan ceza sonuçlarını PDF formatında kaydetme
- Tüm parametreleri ve açıklamaları içeren detaylı rapor
- Yasal dayanakların otomatik eklenmesi

### 4. Modern Kullanıcı Arayüzü
- WPF ve MVVM mimarisi
- Kullanıcı dostu, sezgisel tasarım
- Sekmeli yapı ile düzenli içerik

## 🛠️ Teknolojiler

- **.NET 8.0**: Modern .NET framework
- **WPF (Windows Presentation Foundation)**: Kullanıcı arayüzü
- **MVVM Pattern**: Temiz kod mimarisi
- **CommunityToolkit.Mvvm**: MVVM implementasyonu
- **SQLite**: Yargıtay kararları veritabanı
- **iTextSharp**: PDF rapor oluşturma

## 📋 Gereksinimler

- Windows 10 veya üzeri
- .NET 8.0 Runtime veya SDK

## 🚀 Kurulum

1. Repoyu klonlayın:
```bash
git clone https://github.com/acaroktay1967-crypto/H-rs-zl-k_Suclar-_Analizi.git
cd H-rs-zl-k_Suclar-_Analizi/HirsizlikSuclariAnalizi
```

2. Projeyi derleyin:
```bash
dotnet build
```

3. Uygulamayı çalıştırın:
```bash
dotnet run
```

## 📖 Kullanım

### Ceza Hesaplama

1. **Suç Türü**: Basit veya nitelikli hırsızlık seçin
2. **Mal Değeri**: Çalınan malın değerini TL olarak girin
3. **Nitelikli Haller**: Uygun olan nitelikli halleri işaretleyin
4. **Özel Durumlar**: Etkin pişmanlık, uzlaştırma gibi durumları belirtin
5. **Ceza Hesapla**: Butona tıklayarak ceza hesaplamasını yapın
6. **PDF Oluştur**: Sonuçları PDF olarak kaydedin

### Yargıtay Kararları

1. **Arama**: Arama kutusuna anahtar kelime girin
2. **Listeden Seçim**: Sol taraftaki listeden bir karar seçin
3. **Detay Görüntüleme**: Sağ tarafta karar detaylarını görüntüleyin

## 📁 Proje Yapısı

```
HirsizlikSuclariAnalizi/
├── Models/                    # Veri modelleri
│   ├── HirsizlikTuru.cs
│   ├── NitelikliBolum.cs
│   ├── CezaHesaplamaParametreleri.cs
│   ├── CezaSonucu.cs
│   └── YargitayKarari.cs
├── ViewModels/                # MVVM ViewModels
│   └── MainViewModel.cs
├── Views/                     # XAML görünümleri
│   └── MainWindow.xaml
├── Services/                  # İş mantığı servisleri
│   ├── CezaHesaplamaServisi.cs
│   └── PdfRaporServisi.cs
├── Data/                      # Veritabanı erişimi
│   └── YargitayVeriTabaniServisi.cs
└── Converters/                # XAML Value Converters
    └── ValueConverters.cs
```

## ⚖️ Yasal Uyarı

**ÖNEMLİ**: Bu uygulama yalnızca **bilgilendirme ve eğitim amaçlıdır**. 

- Hesaplanan cezalar **yaklaşık değerlerdir** ve hukuki bağlayıcılığı yoktur
- Kesin ceza tayini mahkemeler tarafından yapılır
- Somut olayın tüm özellikleri ve deliller dikkate alınır
- Hukuki danışmanlık için mutlaka bir avukata başvurunuz

## 📝 TCK İlgili Maddeler

### TCK Madde 141 - Hırsızlık
Zilyedinin rızası olmadan başkasına ait taşınır bir malı, kendisine veya başkasına bir yarar sağlamak maksadıyla bulunduğu yerden alan kimseye **bir yıldan üç yıla kadar hapis cezası** verilir.

### TCK Madde 142 - Nitelikli Haller
Hırsızlık suçunun belirli hallerde işlenmesi durumunda ceza **üçte birden yarısına kadar artırılır**.

## 🤝 Katkıda Bulunma

1. Bu repoyu fork edin
2. Yeni bir branch oluşturun (`git checkout -b feature/AmazingFeature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add some AmazingFeature'`)
4. Branch'inizi push edin (`git push origin feature/AmazingFeature`)
5. Pull Request oluşturun

## 📄 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakınız.

## 👤 Geliştirici

Proje, Türk Ceza Hukuku uzmanları ve yazılım geliştiricilerin iş birliğiyle geliştirilmiştir.

## 📧 İletişim

Sorularınız ve önerileriniz için GitHub Issues kullanabilirsiniz.

---

**Not**: Bu uygulama akademik ve eğitim amaçlı geliştirilmiştir. Profesyonel hukuki danışmanlığın yerini almaz.

