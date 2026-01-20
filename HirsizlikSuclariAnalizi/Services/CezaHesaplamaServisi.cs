using HirsizlikSuclariAnalizi.Models;

namespace HirsizlikSuclariAnalizi.Services
{
    /// <summary>
    /// TCK 141 ve 142. maddelere göre hırsızlık suçları için ceza hesaplama servisi
    /// </summary>
    public class CezaHesaplamaServisi
    {
        /// <summary>
        /// Verilen parametrelere göre ceza hesaplar
        /// </summary>
        public CezaSonucu CezaHesapla(CezaHesaplamaParametreleri parametreler)
        {
            var sonuc = new CezaSonucu();
            
            // TCK 141 - Basit Hırsızlık: 1 yıldan 3 yıla kadar hapis
            int temelAltSinir = 12; // ay
            int temelUstSinir = 36; // ay
            
            sonuc.TemelCezaAltSinir = temelAltSinir;
            sonuc.TemelCezaUstSinir = temelUstSinir;
            sonuc.YasalDayanaklar.Add("TCK Madde 141 - Basit Hırsızlık");
            sonuc.Aciklamalar.Add("Temel ceza: 1 yıldan (12 ay) 3 yıla (36 ay) kadar hapis");
            
            // Nitelikli hırsızlık hallerini kontrol et
            double artirmOrani = 1.0;
            
            foreach (var nitelik in parametreler.NitelikliSebepler.Where(n => n != NitelikliBolum.Yok))
            {
                switch (nitelik)
                {
                    case NitelikliBolum.GeceVakti:
                    case NitelikliBolum.BirlikteSuc:
                    case NitelikliBolum.SavunmasizMagdur:
                    case NitelikliBolum.KamuBinasi:
                    case NitelikliBolum.FelaketDurumu:
                        // TCK 142/1: 1/3 ila 1/2 oranında artırım
                        artirmOrani = Math.Max(artirmOrani, 1.5);
                        sonuc.YasalDayanaklar.Add("TCK Madde 142/1 - Nitelikli Hırsızlık (1/2 artırım)");
                        sonuc.Aciklamalar.Add($"Nitelikli hal ({GetNitelikliAciklama(nitelik)}): Ceza 1/2 oranında artırılır");
                        break;
                        
                    case NitelikliBolum.DiniYer:
                    case NitelikliBolum.HizmetYeri:
                    case NitelikliBolum.MeskenIsyeri:
                        // TCK 142/2: 1/5 ila 1/3 oranında artırım
                        artirmOrani = Math.Max(artirmOrani, 1.3);
                        sonuc.YasalDayanaklar.Add("TCK Madde 142/2 - Nitelikli Hırsızlık (1/3 artırım)");
                        sonuc.Aciklamalar.Add($"Nitelikli hal ({GetNitelikliAciklama(nitelik)}): Ceza 1/3 oranında artırılır");
                        break;
                }
            }
            
            sonuc.ArtirimisCezaAltSinir = (int)(temelAltSinir * artirmOrani);
            sonuc.ArtirimisCezaUstSinir = (int)(temelUstSinir * artirmOrani);
            
            int nihaiAlt = sonuc.ArtirimisCezaAltSinir;
            int nihaiUst = sonuc.ArtirimisCezaUstSinir;
            
            // Teşebbüs hali kontrolü (TCK 35)
            if (parametreler.Tesebbüs)
            {
                nihaiAlt = (int)(nihaiAlt * 0.5); // 1/4 ila 3/4 arası indirim (ortalamasını alıyoruz)
                nihaiUst = (int)(nihaiUst * 0.75);
                sonuc.YasalDayanaklar.Add("TCK Madde 35 - Teşebbüs");
                sonuc.Aciklamalar.Add("Teşebbüs aşamasında kaldığı için ceza 1/4 ila 3/4 oranında indirilir");
            }
            
            // Etkin pişmanlık (TCK 168)
            if (parametreler.EtkinPismanlik)
            {
                nihaiAlt = (int)(nihaiAlt * 0.5); // 2/3 indirim
                nihaiUst = (int)(nihaiUst * 0.67);
                sonuc.YasalDayanaklar.Add("TCK Madde 168 - Etkin Pişmanlık");
                sonuc.Aciklamalar.Add("Etkin pişmanlık nedeniyle ceza 2/3 oranına kadar indirilir");
            }
            
            // Uzlaştırma durumu
            if (parametreler.Uzlastirma)
            {
                sonuc.YasalDayanaklar.Add("CMK Madde 253 - Uzlaştırma");
                sonuc.Aciklamalar.Add("Uzlaştırma sağlandığı takdirde kamu davası açılmaz veya düşürülür");
            }
            
            // Akraba arasında hırsızlık (TCK 167)
            if (parametreler.AkrabaMagdur)
            {
                sonuc.YasalDayanaklar.Add("TCK Madde 167 - Şikayete Tabi Olma");
                sonuc.Aciklamalar.Add("Üstsoy veya altsoydan birine karşı işlendiği takdirde şikayete tabidir");
            }
            
            // Hükmün açıklanmasının geri bırakılması (HAGB)
            if (parametreler.HukmunAciklanmasininGeribirakılmasi)
            {
                sonuc.YasalDayanaklar.Add("CMK Madde 231 - Hükmün Açıklanmasının Geri Bırakılması");
                sonuc.Aciklamalar.Add("Koşulları varsa sanık hakkında HAGB kararı verilebilir (2 yıl ve altı ceza)");
            }
            
            // Mal değerine göre azami değer kontrolü
            if (parametreler.MalDegeri > 0)
            {
                sonuc.Aciklamalar.Add($"Çalınan mal değeri: {parametreler.MalDegeri:N2} TL");
                
                // Küçük değerli hırsızlık TCK 145 (3 aylık kavram kaldırılmıştır, sadece bilgi amaçlı)
                if (parametreler.MalDegeri < 1000)
                {
                    sonuc.Aciklamalar.Add("Not: Mal değeri düşük olsa da basit hırsızlık suçu oluşur");
                }
            }
            
            sonuc.NihaiCezaAltSinir = Math.Max(nihaiAlt, 1); // En az 1 ay
            sonuc.NihaiCezaUstSinir = Math.Max(nihaiUst, 1);
            
            // Para cezasına çevirme (kısa süreli hapis cezalarının ertelenmesi)
            if (sonuc.NihaiCezaUstSinir <= 12) // 1 yıl veya altı
            {
                sonuc.ParaCezasi = parametreler.MalDegeri * 2; // Basit hesaplama
                sonuc.Aciklamalar.Add("Kısa süreli hapis cezası seçenek yaptırıma çevrilebilir");
            }
            
            return sonuc;
        }
        
        private string GetNitelikliAciklama(NitelikliBolum nitelik)
        {
            return nitelik switch
            {
                NitelikliBolum.GeceVakti => "Gece vakti işlenmesi",
                NitelikliBolum.BirlikteSuc => "İki veya daha fazla kişi ile birlikte",
                NitelikliBolum.SavunmasizMagdur => "Savunmasız mağdura karşı",
                NitelikliBolum.KamuBinasi => "Kamu binasında",
                NitelikliBolum.FelaketDurumu => "Felaket durumundan yararlanarak",
                NitelikliBolum.DiniYer => "Dini mahalde",
                NitelikliBolum.HizmetYeri => "Eğitim/sağlık hizmeti verilen yerde",
                NitelikliBolum.MeskenIsyeri => "Mesken veya işyerinde",
                _ => "Nitelikli hal"
            };
        }
    }
}
