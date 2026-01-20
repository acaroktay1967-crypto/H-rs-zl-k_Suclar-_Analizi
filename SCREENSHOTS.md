# Uygulama Ekran Görüntüleri ve Kullanım Örnekleri

## Ana Pencere

Uygulama başlatıldığında modern, profesyonel bir arayüz karşılar:

### Başlık Bölümü
- Mavi arka planlı başlık bandı
- "HIRSIZLIK SUÇLARI ANALİZİ VE CEZA HESAPLAMA SİSTEMİ" başlığı
- Alt başlık: "TCK Madde 141-142 | Yargıtay İçtihatları | PDF Rapor"

### Sekmeler
Uygulamada 3 ana sekme bulunur:
1. Ceza Hesaplama
2. Yargıtay Kararları
3. Hakkında

---

## Sekme 1: Ceza Hesaplama

### Sol Panel: Giriş Parametreleri

#### Suç Türü Bölümü
- İki radio button:
  - [•] Basit Hırsızlık (TCK 141)
  - [ ] Nitelikli Hırsızlık (TCK 142)

#### Mal Değeri Bölümü
- TextBox: [5000] TL

#### Nitelikli Haller - TCK 142/1 (1/2 Artırım)
Beyaz kutulu grup içinde:
- [✓] Gece vakti işlenmiş olması (TCK 142/1-a)
- [ ] İki veya daha fazla kişi tarafından birlikte (TCK 142/1-b)
- [ ] Savunamayacak durumda bulunan kişiye karşı (TCK 142/1-c)
- [ ] Kamu binasında veya eklentilerinde (TCK 142/1-d)
- [ ] Felaket durumlarından yararlanarak (TCK 142/1-e)

#### Nitelikli Haller - TCK 142/2 (1/3 Artırım)
Beyaz kutulu grup içinde:
- [ ] Dini hassasiyetle bağlantılı yerde (TCK 142/2-a)
- [ ] Eğitim, sağlık veya sosyal hizmet yerinde (TCK 142/2-b)
- [✓] Mesken, işyeri, iş makinesi veya eklentilerinde (TCK 142/2-c)

#### Özel Durumlar
- [✓] Etkin Pişmanlık (TCK 168)
- [ ] Uzlaştırma (CMK 253)
- [ ] Akraba Mağdur - Şikayete Tabi (TCK 167)
- [ ] Teşebbüs Aşamasında (TCK 35)
- [ ] HAGB Uygulanabilir (CMK 231)

#### Butonlar
- [Ceza Hesapla] - Mavi buton (150px genişlik)
- [Temizle] - Gri buton (120px genişlik)

### Sağ Panel: Hesaplama Sonuçları

Mavi kenarlı, açık mavi arka planlı sonuç kutusu:

#### TEMEL CEZA
Ceza Aralığı: **12 - 36 ay hapis**

---

#### ARTIRIMIŞ CEZA
Ceza Aralığı: **18 - 54 ay hapis**

---

#### NİHAİ CEZA (Kırmızı vurgu)
**9 - 27 ay hapis**

Alternatif Para Cezası: 10,000.00 TL

---

#### AÇIKLAMALAR
• Temel ceza: 1 yıldan (12 ay) 3 yıla (36 ay) kadar hapis
• Nitelikli hal (Gece vakti işlenmesi): Ceza 1/2 oranında artırılır
• Nitelikli hal (Mesken veya işyerinde): Ceza 1/3 oranında artırılır
• Etkin pişmanlık nedeniyle ceza 2/3 oranına kadar indirilir
• Çalınan mal değeri: 5,000.00 TL
• Kısa süreli hapis cezası seçenek yaptırıma çevrilebilir

---

#### YASAL DAYANAKLAR
• TCK Madde 141 - Basit Hırsızlık
• TCK Madde 142/1 - Nitelikli Hırsızlık (1/2 artırım)
• TCK Madde 142/2 - Nitelikli Hırsızlık (1/3 artırım)
• TCK Madde 168 - Etkin Pişmanlık

#### [PDF Rapor Oluştur] - Yeşil buton (180px)

---

## Sekme 2: Yargıtay Kararları

### Üst Bölüm: Arama

Beyaz kutulu grup içinde:
- TextBox: [gece vakti] (400px genişlik)
- [Ara] - Mavi buton
- [Tümünü Göster] - Gri buton

### Alt Bölüm: Liste ve Detay (İki kolon)

#### Sol Kolon: Kararlar Listesi

**Karar 1:**
```
4. Ceza Dairesi
Esas: 2020/12345 / Karar: 2020/6789
15.05.2020
Gece vakti işlenen hırsızlık suçunda nitelikli hal uygulanması
142/1-a
```

**Karar 2:**
```
4. Ceza Dairesi
Esas: 2019/8765 / Karar: 2020/1234
20.03.2020
İki veya daha fazla kişi ile birlikte işlenen hırsızlık
142/1-b
```

**Karar 3:**
```
4. Ceza Dairesi
Esas: 2021/3456 / Karar: 2021/7890
10.09.2021
Etkin pişmanlık hükümlerinin uygulanması
168
```

#### Sağ Kolon: Karar Detayı

**4. Ceza Dairesi**

Esas No: 2020/12345
Karar No: 2020/6789
Tarih: 15.05.2020
TCK Maddesi: 142/1-a

---

**ÖZET**
Gece vakti işlenen hırsızlık suçunda nitelikli hal uygulanması

---

**KARAR METNİ**
Sanığın, gece vakti mağdurun evine girerek hırsızlık yapması nedeniyle TCK 142/1-a maddesinin uygulanması gerektiği, mahkemenin basit hırsızlık olarak kabul etmesinin hatalı olduğu...

---

**ANAHTAR KELİMELER**
gece vakti, nitelikli hırsızlık, mesken

---

## Sekme 3: Hakkında

Beyaz arka plan, sol hizalı metin:

### HIRSIZLIK SUÇLARI ANALİZİ VE CEZA HESAPLAMA SİSTEMİ

Bu uygulama, Türk Ceza Kanunu'nun 141 ve 142. maddelerine göre hırsızlık suçlarının analizini ve ceza hesaplamasını yapmanızı sağlar.

**Özellikler:**
• Basit ve nitelikli hırsızlık türlerini destekler
• TCK 141 ve 142. maddelere göre ceza hesaplaması
• Nitelikli hallerin otomatik değerlendirilmesi
• Etkin pişmanlık, uzlaştırma gibi indirim sebeplerinin hesaplanması
• Yargıtay kararlarına erişim ve arama
• PDF rapor oluşturma

**Yasal Uyarı:** (Kırmızı metin, bold)
Bu uygulama yalnızca bilgilendirme ve eğitim amaçlıdır. Hesaplanan cezalar yaklaşık değerlerdir ve hukuki bağlayıcılığı yoktur...

**TCK İlgili Maddeler:**

TCK Madde 141 - Hırsızlık:
(1) Zilyedinin rızası olmadan başkasına ait taşınır bir malı...

TCK Madde 142 - Nitelikli Haller:
(1) Hırsızlık suçunun;
a) Gece vakti,
b) İki veya daha fazla kişi tarafından birlikte...

Versiyon: 1.0.0
© 2024 - Hırsızlık Suçları Analizi Sistemi

---

## Örnek Kullanım Akışı

### Senaryo: Gece vakti mesken hırsızlığı, etkin pişmanlık var

1. **Ceza Hesaplama** sekmesini aç
2. Parametreleri gir:
   - Mal Değeri: 15000 TL
   - Gece vakti: ✓
   - Mesken/işyeri: ✓
   - Etkin Pişmanlık: ✓
3. **[Ceza Hesapla]** butonuna tıkla
4. Sonuçlar sağ panelde görüntülenir:
   - Temel: 12-36 ay
   - Artırılmış: 23-70 ay (kombine nitelikli haller)
   - Nihai: 11-46 ay (etkin pişmanlık indirimi)
5. **[PDF Rapor Oluştur]** ile kaydet
6. Dosya konumu seç ve kaydet
7. Başarı mesajı alınır

### Senaryo: Yargıtay kararı araştırma

1. **Yargıtay Kararları** sekmesini aç
2. Arama kutusuna "mesken" yaz
3. **[Ara]** butonuna tıkla
4. Sonuçlar listede görüntülenir
5. Bir karar seç
6. Detaylar sağ panelde görüntülenir
7. Karar metnini oku

---

## Renk Paleti

- **Birincil Mavi**: #0078D4 (Microsoft Blue)
- **Hover Mavi**: #005A9E
- **Arka Plan**: #F5F5F5 (Açık Gri)
- **Başarı Yeşil**: #28A745
- **Uyarı Kırmızı**: #D9534F
- **Gri**: #6C757D
- **Kenarlık**: #CCCCCC
- **Beyaz**: #FFFFFF

## Yazı Tipleri

- Başlık: 24pt, Bold
- Alt Başlık: 14-16pt, Bold
- Normal Metin: 11-12pt
- Küçük Metin: 9-10pt

## Pencere Boyutları

- Genişlik: 1400px
- Yükseklik: 900px
- Başlangıç Konumu: Ekran ortası
- Yeniden boyutlandırılabilir: Evet
