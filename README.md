# Hırsızlık Suçları Analizi

Türk Ceza Kanunu'nun 141 ve 142. maddelerine uygun şekilde hırsızlık suçlarına ilişkin analiz ve ceza hesaplamaları için geliştirilmiş modern bir WPF uygulaması.

## Özellikler

### 1. Hırsızlık Türü Seçimi
- **Basit Hırsızlık (TCK 141)**: Başkasına ait taşınır bir malı, zilyedinin rızası olmaksızın alma
- **Nitelikli Hırsızlık (TCK 142)**: Belirli hallerde işlenen hırsızlık suçu (ceza yarı oranında artırılır)

### 2. Ceza Hesaplama Modülü
- Mal değerine göre otomatik ceza hesaplama
- Gece vakti işlenme durumunda ek ceza hesaplama
- Etkin pişmanlık indirim hesaplaması
- Detaylı yasal dayanak açıklamaları
- Hesaplama sonuçlarını veritabanına kaydetme

### 3. Yargıtay Kararları Modülü
- Karar numarasına göre filtreleme
- Suç türüne göre filtreleme
- Kararların detaylı incelenmesi
- PDF formatında karar raporları

### 4. PDF Raporlama
- Ceza hesaplama raporları
- Yargıtay karar raporları
- Yasal dayanaklarla birlikte detaylı çıktılar

## Teknolojiler

- **Framework**: .NET 8.0 WPF
- **Veritabanı**: SQLite
- **PDF Oluşturma**: QuestPDF
- **UI**: Modern ve kullanıcı dostu WPF tasarımı

## Kurulum ve Çalıştırma

### Gereksinimler
- .NET 8.0 SDK veya üzeri
- Windows işletim sistemi

### Derleme ve Çalıştırma

```bash
# Proje dizinine gidin
cd HirsizlikSuclariAnalizi

# Bağımlılıkları yükleyin
dotnet restore

# Projeyi derleyin
dotnet build

# Uygulamayı çalıştırın
dotnet run
```

## Kullanım

### Ceza Hesaplama
1. Ana menüden "📊 Ceza Hesaplama" seçeneğini tıklayın
2. Hırsızlık türünü seçin (Basit veya Nitelikli)
3. Mal değerini TL cinsinden girin
4. Gerekirse ek faktörleri işaretleyin (Gece vakti, Etkin pişmanlık)
5. "Ceza Hesapla" butonuna tıklayın
6. Sonuçları "Kaydet" veya "PDF Oluştur" seçenekleriyle saklayın

### Yargıtay Kararları
1. Ana menüden "⚖️ Yargıtay Kararları" seçeneğini tıklayın
2. Filtreleme seçeneklerini kullanarak karar arayın
3. Listeden bir karar seçerek detaylarını görüntüleyin
4. İstediğiniz kararı PDF olarak kaydedin

## Yasal Dayanak

### TCK 141. Madde - Basit Hırsızlık
Başkasına ait taşınır bir malı, zilyedinin rızası olmaksızın kendisine veya başkasına bir yarar sağlamak maksadıyla bulunduğu yerden alan kimseye bir yıldan üç yıla kadar hapis cezası verilir.

### TCK 142. Madde - Nitelikli Hırsızlık
Hırsızlık suçunun belirli hallerde (gece vakti, konut dokunulmazlığını ihlal ederek vb.) işlenmesi durumunda ceza yarı oranında artırılır.

## Lisans

Bu proje MIT lisansı altında lisanslanmıştır.

