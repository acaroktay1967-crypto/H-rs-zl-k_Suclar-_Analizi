# Uygulama Arayüzü ve İşlevsellik Dokümantasyonu

## Arayüz Görünümü

```
┌──────────────────────────────────────────────────────────────────────────────────┐
│ Hırsızlık Suçları Analizi - Kanun Görüntüleyici                          [_][□][X]│
├──────────────────────────────────────────────────────────────────────────────────┤
│                                                                                   │
│  ┌─────┐  ┌─────┐  ┌─────┐  ┌─────┐  ┌─────┐  ┌─────────┐                     │
│  │ TCK │  │ CMK │  │ TMK │  │ TBK │  │ ÇKK │  │ Temizle │                     │
│  └─────┘  └─────┘  └─────┘  └─────┘  └─────┘  └─────────┘                     │
│   Mavi     Yeşil   Turuncu  Mor      Kırmızı    Gri                              │
│                                                                                   │
├───────────────────────────────────────────────────────────────────────────────────┤
│ ┌─────────────────────────────────────────────────────────────────────────────┐ │
│ │                                                                             │ │
│ │  RichTextBox - Kanun Metni Görüntüleme Alanı                              │ │
│ │                                                                             │ │
│ │  Kanun görüntülemek için yukarıdaki butonlardan birini seçin.            │ │
│ │                                                                             │ │
│ │                                                                             │ │
│ │  [Buton tıklandığında buraya ilgili kanun metni yüklenir]                 │ │
│ │                                                                             │ │
│ │  Örnek: TCK butonuna tıklandığında:                                        │ │
│ │                                                                             │ │
│ │  TÜRK CEZA KANUNU (TCK)                                                    │ │
│ │  Kanun Numarası: 5237                                                      │ │
│ │  Kabul Tarihi: 26/09/2004                                                  │ │
│ │                                                                             │ │
│ │  BİRİNCİ KİTAP                                                             │ │
│ │  Genel Hükümler                                                            │ │
│ │  ...                                                                        │ │
│ │                                                                             ▲│
│ │                                                                             ││
│ │                                                                             ││
│ │                                                                             ▼│
│ └─────────────────────────────────────────────────────────────────────────────┘ │
│                                                                                   │
└───────────────────────────────────────────────────────────────────────────────────┘
```

## Buton Detayları ve Renk Kodları

1. **TCK Butonu** - `#FF2196F3` (Mavi)
   - Tooltip: "Türk Ceza Kanunu"
   - Dosya: `kanunlar/TCK.txt`
   - İçerik: 5237 Sayılı Türk Ceza Kanunu

2. **CMK Butonu** - `#FF4CAF50` (Yeşil)
   - Tooltip: "Ceza Muhakemesi Kanunu"
   - Dosya: `kanunlar/CMK.txt`
   - İçerik: 5271 Sayılı Ceza Muhakemesi Kanunu

3. **TMK Butonu** - `#FFFF9800` (Turuncu)
   - Tooltip: "Türk Medeni Kanunu"
   - Dosya: `kanunlar/TMK.txt`
   - İçerik: 4721 Sayılı Türk Medeni Kanunu

4. **TBK Butonu** - `#FF9C27B0` (Mor)
   - Tooltip: "Türk Borçlar Kanunu"
   - Dosya: `kanunlar/TBK.txt`
   - İçerik: 6098 Sayılı Türk Borçlar Kanunu

5. **ÇKK Butonu** - `#FFF44336` (Kırmızı)
   - Tooltip: "Çocuk Koruma Kanunu"
   - Dosya: `kanunlar/CIK.txt`
   - İçerik: 5395 Sayılı Çocuk Koruma Kanunu

6. **Temizle Butonu** - `#FF607D8B` (Gri)
   - Tooltip: "Metin editörünü temizle"
   - İşlev: RichTextBox'ı temizler ve başlangıç mesajını gösterir

## İşlevsellik Açıklaması

### Buton Tıklama Olayları
Her buton tıklandığında aşağıdaki işlemler gerçekleşir:

1. **LoadKanunText(fileName)** metodu çağrılır
2. Uygulama dizininde `kanunlar/` klasörü içinden ilgili dosya bulunur
3. Dosya UTF-8 encoding ile okunur
4. RichTextBox temizlenir
5. Okunan metin RichTextBox'a yüklenir
6. Hata durumunda kullanıcıya MessageBox ile bilgi verilir

### Hata Yönetimi
- Dosya bulunamazsa: "Kanun dosyası bulunamadı" hatası
- Dosya okunamazsa: "Kanun dosyası okunurken hata oluştu" hatası
- Tüm hatalar kullanıcıya anlaşılır MessageBox ile gösterilir

## Mimari

```
HirsizlikSucAnalizApp/
├── App.xaml                    (Uygulama tanımı)
├── App.xaml.cs                 (Uygulama kodu)
├── MainWindow.xaml             (Ana pencere UI tanımı)
├── MainWindow.xaml.cs          (Ana pencere kodu ve event handler'lar)
├── AssemblyInfo.cs             (Assembly bilgileri)
├── HirsizlikSucAnalizApp.csproj (Proje dosyası)
└── kanunlar/                   (Kanun metinleri klasörü)
    ├── TCK.txt                 (Türk Ceza Kanunu)
    ├── CMK.txt                 (Ceza Muhakemesi Kanunu)
    ├── TMK.txt                 (Türk Medeni Kanunu)
    ├── TBK.txt                 (Türk Borçlar Kanunu)
    └── CIK.txt                 (Çocuk Koruma Kanunu)
```

## Kullanım Senaryoları

### Senaryo 1: TCK Görüntüleme
1. Kullanıcı uygulamayı açar
2. Mavi "TCK" butonuna tıklar
3. Türk Ceza Kanunu metni metin editöründe görüntülenir
4. Kullanıcı metni okuyabilir, kaydırabilir

### Senaryo 2: Farklı Kanunlar Arası Geçiş
1. Kullanıcı CMK butonuna tıklar
2. Ceza Muhakemesi Kanunu görüntülenir
3. Kullanıcı TMK butonuna tıklar
4. Önceki içerik temizlenir, Türk Medeni Kanunu görüntülenir

### Senaryo 3: Metin Editörünü Temizleme
1. Kullanıcı herhangi bir kanunu görüntüler
2. "Temizle" butonuna tıklar
3. Metin editörü temizlenir
4. Başlangıç mesajı gösterilir

## Geliştirme Notları

- **Framework**: .NET 8.0 WPF
- **UI Pattern**: Code-behind pattern
- **Encoding**: UTF-8 (Türkçe karakter desteği için)
- **Error Handling**: Try-catch blokları ile merkezi hata yönetimi
- **File Management**: AppDomain.CurrentDomain.BaseDirectory kullanılarak dinamik path belirleme
