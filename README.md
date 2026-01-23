# Hırsızlık Suçları Analizi - Kanun Görüntüleyici

Bu proje, Türk hukuk sistemindeki önemli kanunları görüntülemek için tasarlanmış bir WPF (Windows Presentation Foundation) uygulamasıdır.

## Özellikler

Uygulama aşağıdaki kanunların tam metinlerini görüntüleme imkanı sağlar:
- **TCK** - Türk Ceza Kanunu (5237 Sayılı Kanun)
- **CMK** - Ceza Muhakemesi Kanunu (5271 Sayılı Kanun)
- **TMK** - Türk Medeni Kanunu (4721 Sayılı Kanun)
- **TBK** - Türk Borçlar Kanunu (6098 Sayılı Kanun)
- **ÇKK** - Çocuk Koruma Kanunu (5395 Sayılı Kanun)

## Kullanım

1. Uygulamayı çalıştırın
2. Üst kısımda bulunan renkli butonlardan görüntülemek istediğiniz kanunu seçin
3. Seçtiğiniz kanunun tam metni metin editöründe görüntülenecektir
4. "Temizle" butonu ile metin editörünü temizleyebilirsiniz

## Teknik Detaylar

- **Platform**: .NET 8.0 (Windows)
- **UI Framework**: WPF
- **Kanun Metinleri**: `HirsizlikSucAnalizApp/kanunlar/` dizininde metin dosyaları olarak saklanır
- **Metin Editörü**: RichTextBox bileşeni kullanılır

## Geliştirme

Projeyi derlemek için:
```bash
cd HirsizlikSucAnalizApp
dotnet build
```

Projeyi çalıştırmak için:
```bash
cd HirsizlikSucAnalizApp
dotnet run
```

## Kanun Metinlerini Güncelleme

Kanun metinlerini güncellemek veya yeni kanunlar eklemek için:
1. `HirsizlikSucAnalizApp/kanunlar/` dizinine yeni `.txt` dosyası ekleyin
2. İlgili buton ve event handler'ı XAML ve C# koduna ekleyin
3. Projeyi yeniden derleyin

