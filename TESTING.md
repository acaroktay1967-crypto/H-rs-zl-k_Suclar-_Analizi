# Test ve Kullanım Kılavuzu

## Test Projesi

Proje, temel işlevselliği doğrulamak için bir konsol test uygulaması içermektedir: `HirsizlikSuclariAnalizi.Tests`

### Test Kapsamı

Test projesi aşağıdaki fonksiyonları doğrular:

1. **Basit Hırsızlık Testi**: TCK 141 maddesine göre temel ceza hesaplama
2. **Nitelikli Hırsızlık Testi**: TCK 142/1 maddesine göre gece vakti artırımı
3. **Etkin Pişmanlık Testi**: TCK 168 maddesine göre indirim hesaplama
4. **Yargıtay Veritabanı Testi**: SQLite veritabanı işlemleri ve arama
5. **PDF Oluşturma Testi**: PDF rapor oluşturma fonksiyonalitesi

### Test Çalıştırma

Windows sisteminde:
```bash
cd HirsizlikSuclariAnalizi.Tests
dotnet run
```

### Beklenen Test Sonuçları

#### Test 1: Basit Hırsızlık
- Temel Ceza: 12 - 36 ay
- Nihai Ceza: 12 - 36 ay
- Açıklama sayısı: 1+

#### Test 2: Nitelikli Hırsızlık (Gece Vakti)
- Temel Ceza: 12 - 36 ay
- Artırılmış Ceza: 18 - 54 ay (1.5x artırım)
- Nihai Ceza: 18 - 54 ay

#### Test 3: Etkin Pişmanlık
- Temel Ceza: 12 - 36 ay
- İndirimli Ceza: ~6 - 24 ay (indirim uygulanmış)
- Etkin Pişmanlık İndirimi: Evet

#### Test 4: Yargıtay Veritabanı
- Toplam Karar: 5 karar
- 'Gece vakti' araması: 1+ sonuç

#### Test 5: PDF Oluşturma
- PDF dosyası başarıyla oluşturulur
- Dosya boyutu: >0 bytes

## WPF Uygulaması Manuel Test Senaryoları

### Senaryo 1: Basit Hırsızlık Hesaplama

1. Uygulamayı başlatın
2. "Ceza Hesaplama" sekmesine gidin
3. "Basit Hırsızlık (TCK 141)" seçin
4. Mal Değeri: 5000 TL girin
5. "Ceza Hesapla" butonuna tıklayın
6. **Beklenen Sonuç**: 12-36 ay hapis cezası görüntülenir

### Senaryo 2: Nitelikli Hırsızlık (Gece Vakti + Mesken)

1. "Nitelikli Hırsızlık (TCK 142)" seçin
2. Mal Değeri: 10000 TL
3. "Gece vakti işlenmiş olması" işaretleyin
4. "Mesken veya işyerinde" işaretleyin
5. "Ceza Hesapla" butonuna tıklayın
6. **Beklenen Sonuç**: Artırılmış ceza (18-54 ay) görüntülenir

### Senaryo 3: Etkin Pişmanlık ve İndirimler

1. Basit hırsızlık seçin
2. Mal Değeri: 3000 TL
3. "Etkin Pişmanlık" işaretleyin
4. "Teşebbüs Aşamasında" işaretleyin
5. "Ceza Hesapla" butonuna tıklayın
6. **Beklenen Sonuç**: İndirimli ceza ve açıklamalar görüntülenir

### Senaryo 4: PDF Rapor Oluşturma

1. Herhangi bir ceza hesaplama yapın
2. "PDF Rapor Oluştur" butonuna tıklayın
3. Dosya adı ve konum seçin
4. **Beklenen Sonuç**: PDF dosyası oluşturulur ve onay mesajı gösterilir

### Senaryo 5: Yargıtay Kararları Arama

1. "Yargıtay Kararları" sekmesine gidin
2. Arama kutusuna "gece vakti" yazın
3. "Ara" butonuna tıklayın
4. **Beklenen Sonuç**: İlgili kararlar listelenir
5. Bir karar seçin
6. **Beklenen Sonuç**: Karar detayları sağ panelde görüntülenir

### Senaryo 6: Tüm Özelliklerin Kombinasyonu

1. Nitelikli hırsızlık seçin
2. Mal Değeri: 20000 TL
3. Şu seçenekleri işaretleyin:
   - Gece vakti
   - İki veya daha fazla kişi
   - Mesken/işyeri
   - Etkin pişmanlık
4. "Ceza Hesapla" butonuna tıklayın
5. Sonuçları inceleyin
6. "PDF Rapor Oluştur" ile kaydedin
7. **Beklenen Sonuç**: Tüm parametreler doğru şekilde hesaplanır ve PDF'e kaydedilir

## UI Test Kontrol Listesi

### Görsel Kontroller
- [ ] Ana pencere düzgün açılıyor
- [ ] Tüm sekmeler erişilebilir
- [ ] Butonlar ve kontroller düzgün görünüyor
- [ ] Yazı tipleri okunabilir
- [ ] Renkler uyumlu

### Fonksiyonel Kontroller
- [ ] Radio butonlar çalışıyor
- [ ] Checkbox'lar işaretlenebiliyor
- [ ] TextBox'lara veri girilebiliyor
- [ ] Butonlara tıklanabiliyor
- [ ] ComboBox'lar seçim yapılabiliyor
- [ ] ScrollBar'lar çalışıyor

### Veri Doğrulama
- [ ] Geçersiz mal değeri uyarı veriyor mu?
- [ ] Hesaplama sonuçları mantıklı mı?
- [ ] PDF oluşturma başarılı mı?
- [ ] Veritabanı sorguları çalışıyor mu?

## Bilinen Sınırlamalar

1. **Platform**: Sadece Windows üzerinde çalışır (WPF uygulaması)
2. **Veritabanı**: İlk çalıştırmada yargitay.db oluşturulur
3. **PDF**: Türkçe karakterler için özel font gerekebilir
4. **Test Ortamı**: Linux üzerinde WPF çalışmaz, sadece derleme yapılabilir

## Hata Ayıklama

### Uygulama Açılmıyor
- .NET 8.0 Windows Desktop Runtime yüklü mü kontrol edin
- Windows 10/11 kullanıyor musunuz?

### Veritabanı Hatası
- Uygulamanın yazma izni var mı?
- yargitay.db dosyası silinip yeniden oluşturulabilir

### PDF Oluşturulamıyor
- Hedef klasöre yazma izni var mı?
- Dosya adı geçerli mi?
- Disk alanı yeterli mi?

## Performans Beklentileri

- Ceza hesaplama: <100ms
- Veritabanı sorgusu: <50ms
- PDF oluşturma: <500ms
- Arayüz yanıt süresi: <50ms

## Güvenlik Notları

- Veritabanı bağlantısı SQLite parametreli sorgular kullanır
- Kullanıcı girdileri validasyondan geçer
- PDF oluşturma güvenli metodlar kullanır
- Hassas veri şifrelenmez (eğitim amaçlı uygulama)
