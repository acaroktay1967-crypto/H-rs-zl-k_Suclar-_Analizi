# Kullanım Kılavuzu - Hırsızlık Suçları Analiz Sistemi

## İçindekiler
1. [Ceza Hesaplama](#ceza-hesaplama)
2. [Yargıtay Kararları](#yargıtay-kararları)
3. [PDF Raporlama](#pdf-raporlama)
4. [Veritabanı](#veritabanı)

## Ceza Hesaplama

### Basit Hırsızlık Örneği

**Senaryo:** 500 TL değerindeki bir cep telefonu çalınmış.

1. "Ceza Hesaplama" modülünü açın
2. "Basit Hırsızlık (TCK 141)" seçeneğini işaretleyin
3. Mal Değeri alanına "500" girin
4. "Ceza Hesapla" butonuna tıklayın

**Sonuç:** 1 - 1.9 yıl hapis cezası (Düşük mal değeri nedeniyle ceza alt sınırdan uygulanır)

### Nitelikli Hırsızlık Örneği

**Senaryo:** Gece vakti 15.000 TL değerindeki bir ürün çalınmış, ancak fail malı geri vermiş.

1. "Ceza Hesaplama" modülünü açın
2. "Nitelikli Hırsızlık (TCK 142)" seçeneğini işaretleyin
3. Mal Değeri alanına "15000" girin
4. "Gece Vakti İşlenmiş" kutusunu işaretleyin
5. "Etkin Pişmanlık" kutusunu işaretleyin
6. "Ceza Hesapla" butonuna tıklayın

**Sonuç:** Yaklaşık 2.01 - 3.02 yıl hapis cezası
- Nitelikli hırsızlık temel ceza: 1.5 - 4.5 yıl
- Yüksek mal değeri etkisi uygulanır
- Etkin pişmanlık nedeniyle ceza yaklaşık %33 indirilir

### Ceza Hesaplama Formülleri

#### Basit Hırsızlık (TCK 141)
- **Temel Ceza:** 1 - 3 yıl hapis
- **Düşük Mal Değeri (<1.000 TL):** Ceza alt sınırdan uygulanır
- **Yüksek Mal Değeri (>10.000 TL):** Ceza üst sınıra yakın uygulanır

#### Nitelikli Hırsızlık (TCK 142)
- **Temel Ceza:** Basit hırsızlık cezası x 1.5 = 1.5 - 4.5 yıl
- **Gece Vakti İşlenme:** Temel ceza x 1.5
- **Etkin Pişmanlık:** Ceza x 0.67 (yaklaşık %33 indirim)

## Yargıtay Kararları

### Karar Arama

#### Karar Numarasına Göre Arama
1. "Yargıtay Kararları" modülünü açın
2. "Karar Numarası" alanına numarayı girin (örn: "2019/1234")
3. "Filtrele" butonuna tıklayın

#### Suç Türüne Göre Arama
1. "Yargıtay Kararları" modülünü açın
2. "Suç Türü" açılır menüsünden seçim yapın
   - Tümü (varsayılan)
   - Basit Hırsızlık
   - Nitelikli Hırsızlık
3. "Filtrele" butonuna tıklayın

### Karar Detayları

Listeden bir karar seçtiğinizde, detaylı bilgileri içeren bir pencere açılır:
- Karar Numarası
- Karar Tarihi
- Suç Türü
- Özet
- Tam Metin

## PDF Raporlama

### Ceza Hesaplama Raporu

1. Ceza hesaplama yapın
2. "PDF Oluştur" butonuna tıklayın
3. Kayıt konumunu seçin
4. PDF otomatik olarak açılır

**Rapor İçeriği:**
- Rapor tarihi
- Suç bilgileri (tür, mal değeri, faktörler)
- Hesaplanan ceza aralığı
- Yasal dayanak (TCK maddeleri)
- Notlar (varsa)

### Yargıtay Kararı Raporu

1. Bir karar seçin ve detay penceresini açın
2. "PDF Olarak Kaydet" butonuna tıklayın
3. Kayıt konumunu seçin
4. PDF otomatik olarak açılır

**Rapor İçeriği:**
- Karar numarası ve tarihi
- Suç türü
- Karar özeti
- Karar tam metni

## Veritabanı

### Otomatik Oluşturma
Uygulama ilk çalıştırıldığında, SQLite veritabanı otomatik olarak oluşturulur ve örnek Yargıtay kararları ile doldurulur.

### Veritabanı Konumu
```
<Uygulama Dizini>/hirsizlik.db
```

### Örnek Yargıtay Kararları

Sistem aşağıdaki örnek kararlarla gelir:

1. **2019/1234** - Basit hırsızlık suçunda mal değerinin önemi
2. **2020/5678** - Gece vakti işlenen hırsızlık nitelikli hırsızlıktır
3. **2021/9012** - Etkin pişmanlık hükümlerinin uygulanması
4. **2022/3456** - Basit hırsızlıkta zincirleme suç
5. **2022/7890** - Konut dokunulmazlığını ihlalle hırsızlık

### Kayıt Saklama

Tüm ceza hesaplamaları veritabanında saklanır ve daha sonra incelenebilir.

## Yaygın Senaryolar

### Senaryo 1: İşyerinden Gündüz Hırsızlık
- **Durum:** Gündüz saatlerinde işyerinden 2.500 TL değerinde eşya çalınmış
- **Ceza Türü:** Basit Hırsızlık
- **Faktörler:** Orta mal değeri
- **Tahmini Ceza:** 1.6 - 2.4 yıl hapis

### Senaryo 2: Konuttan Gece Hırsızlık
- **Durum:** Gece vakti konuta girilerek 8.000 TL değerinde eşya çalınmış
- **Ceza Türü:** Nitelikli Hırsızlık
- **Faktörler:** Gece vakti, orta-yüksek mal değeri
- **Tahmini Ceza:** 2.1 - 3.6 yıl hapis

### Senaryo 3: Etkin Pişmanlıkla Hırsızlık
- **Durum:** Fail çaldığı 5.000 TL değerindeki eşyayı geri vermiş
- **Ceza Türü:** Basit Hırsızlık
- **Faktörler:** Etkin pişmanlık
- **Tahmini Ceza:** 1.1 - 1.8 yıl hapis (yaklaşık %33 indirim)

## Sık Sorulan Sorular

### Ceza hesaplamaları kesin midir?
Hayır, hesaplamalar TCK maddeleri temel alınarak yapılan tahminlerdir. Gerçek ceza, mahkeme tarafından somut olayın tüm koşulları değerlendirilerek verilir.

### Veritabanı sıfırlanabilir mi?
Evet, `hirsizlik.db` dosyasını silin ve uygulamayı yeniden başlatın. Veritabanı otomatik olarak yeniden oluşturulacaktır.

### Kendi Yargıtay kararlarımı ekleyebilir miyim?
Şu anda uygulama arayüzünden ekleme özelliği yoktur, ancak veritabanına doğrudan SQL komutlarıyla eklenebilir.

### PDF raporları nerede saklanır?
PDF raporları, kaydetme dialogunda seçtiğiniz konuma kaydedilir. Varsayılan bir konum yoktur.

## Teknik Destek

Herhangi bir sorun veya öneri için proje GitHub sayfasını ziyaret edin.
