using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using HirsizlikSuclariAnalizi.Models;

namespace HirsizlikSuclariAnalizi.Services
{
    /// <summary>
    /// PDF rapor oluşturma servisi
    /// </summary>
    public class PdfRaporServisi
    {
        /// <summary>
        /// Ceza hesaplama sonucunu PDF raporu olarak kaydeder
        /// </summary>
        public void RaporOlustur(string dosyaYolu, CezaHesaplamaParametreleri parametreler, CezaSonucu sonuc)
        {
            using var stream = new FileStream(dosyaYolu, FileMode.Create);
            var document = new Document(PageSize.A4);
            var writer = PdfWriter.GetInstance(document, stream);
            
            document.Open();
            
            // Başlık
            var baslikFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, iTextSharp.text.Font.NORMAL);
            var altBaslikFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, iTextSharp.text.Font.NORMAL);
            var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 11, iTextSharp.text.Font.NORMAL);
            var kucukFont = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL);
            
            // Başlık
            var baslik = new Paragraph("HIRSIZLIK SUÇU CEZA HESAPLAMA RAPORU", baslikFont);
            baslik.Alignment = Element.ALIGN_CENTER;
            baslik.SpacingAfter = 20;
            document.Add(baslik);
            
            // Tarih
            var tarih = new Paragraph($"Rapor Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}", kucukFont);
            tarih.Alignment = Element.ALIGN_RIGHT;
            tarih.SpacingAfter = 20;
            document.Add(tarih);
            
            // Suç Bilgileri
            document.Add(new Paragraph("SUÇ BİLGİLERİ", altBaslikFont));
            document.Add(new Paragraph($"Suç Türü: {GetSucTuruAciklama(parametreler)}", normalFont));
            
            if (parametreler.NitelikliSebepler.Any(n => n != NitelikliBolum.Yok))
            {
                document.Add(new Paragraph($"Nitelikli Haller:", normalFont));
                foreach (var nitelik in parametreler.NitelikliSebepler.Where(n => n != NitelikliBolum.Yok))
                {
                    document.Add(new Paragraph($"  • {GetNitelikliAciklama(nitelik)}", normalFont));
                }
            }
            
            if (parametreler.MalDegeri > 0)
            {
                document.Add(new Paragraph($"Çalınan Mal Değeri: {parametreler.MalDegeri:N2} TL", normalFont));
            }
            
            document.Add(new Paragraph(" ", normalFont)); // Boşluk
            
            // Özel Durumlar
            var ozelDurumlar = new List<string>();
            if (parametreler.EtkinPismanlik) ozelDurumlar.Add("Etkin Pişmanlık");
            if (parametreler.Uzlastirma) ozelDurumlar.Add("Uzlaştırma");
            if (parametreler.AkrabaMagdur) ozelDurumlar.Add("Akraba Mağdur");
            if (parametreler.Tesebbüs) ozelDurumlar.Add("Teşebbüs Aşaması");
            if (parametreler.HukmunAciklanmasininGeribirakılmasi) ozelDurumlar.Add("HAGB Uygulanabilir");
            
            if (ozelDurumlar.Any())
            {
                document.Add(new Paragraph("Özel Durumlar:", normalFont));
                foreach (var durum in ozelDurumlar)
                {
                    document.Add(new Paragraph($"  • {durum}", normalFont));
                }
                document.Add(new Paragraph(" ", normalFont));
            }
            
            // Ceza Hesaplaması
            document.Add(new Paragraph("CEZA HESAPLAMASI", altBaslikFont));
            document.Add(new Paragraph(" ", normalFont));
            
            document.Add(new Paragraph($"Temel Ceza: {sonuc.TemelCezaAltSinir} - {sonuc.TemelCezaUstSinir} ay hapis", normalFont));
            
            if (sonuc.ArtirimisCezaAltSinir != sonuc.TemelCezaAltSinir)
            {
                document.Add(new Paragraph($"Nitelikli Hallerle Artırılmış Ceza: {sonuc.ArtirimisCezaAltSinir} - {sonuc.ArtirimisCezaUstSinir} ay hapis", normalFont));
            }
            
            document.Add(new Paragraph(" ", normalFont));
            
            var nihaiCezaBaslik = new Paragraph($"NİHAİ CEZA: {sonuc.NihaiCezaAltSinir} - {sonuc.NihaiCezaUstSinir} ay hapis", altBaslikFont);
            nihaiCezaBaslik.SpacingBefore = 10;
            document.Add(nihaiCezaBaslik);
            
            if (sonuc.ParaCezasi.HasValue)
            {
                document.Add(new Paragraph($"Alternatif Para Cezası: {sonuc.ParaCezasi:N2} TL", normalFont));
            }
            
            document.Add(new Paragraph(" ", normalFont));
            
            // Açıklamalar
            if (sonuc.Aciklamalar.Any())
            {
                document.Add(new Paragraph("AÇIKLAMALAR", altBaslikFont));
                foreach (var aciklama in sonuc.Aciklamalar)
                {
                    document.Add(new Paragraph($"• {aciklama}", kucukFont));
                }
                document.Add(new Paragraph(" ", normalFont));
            }
            
            // Yasal Dayanaklar
            if (sonuc.YasalDayanaklar.Any())
            {
                document.Add(new Paragraph("YASAL DAYANAKLAR", altBaslikFont));
                foreach (var dayanak in sonuc.YasalDayanaklar.Distinct())
                {
                    document.Add(new Paragraph($"• {dayanak}", kucukFont));
                }
            }
            
            // Uyarı
            document.Add(new Paragraph(" ", normalFont));
            document.Add(new Paragraph(" ", normalFont));
            var uyari = new Paragraph(
                "UYARI: Bu rapor bilgilendirme amaçlıdır. Kesin ceza, mahkeme tarafından tüm deliller ve somut olay değerlendirilerek belirlenir.",
                kucukFont
            );
            uyari.Alignment = Element.ALIGN_JUSTIFIED;
            document.Add(uyari);
            
            document.Close();
        }
        
        private string GetSucTuruAciklama(CezaHesaplamaParametreleri parametreler)
        {
            return parametreler.HirsizlikTuru switch
            {
                HirsizlikTuru.BasitHirsizlik => "Basit Hırsızlık (TCK 141)",
                HirsizlikTuru.NitelikliHirsizlik => "Nitelikli Hırsızlık (TCK 142)",
                _ => "Belirsiz"
            };
        }
        
        private string GetNitelikliAciklama(NitelikliBolum nitelik)
        {
            return nitelik switch
            {
                NitelikliBolum.GeceVakti => "Gece vakti işlenmesi (TCK 142/1-a)",
                NitelikliBolum.BirlikteSuc => "İki veya daha fazla kişi ile birlikte (TCK 142/1-b)",
                NitelikliBolum.SavunmasizMagdur => "Savunmasız mağdura karşı (TCK 142/1-c)",
                NitelikliBolum.KamuBinasi => "Kamu binasında (TCK 142/1-d)",
                NitelikliBolum.FelaketDurumu => "Felaket durumundan yararlanarak (TCK 142/1-e)",
                NitelikliBolum.DiniYer => "Dini mahalde (TCK 142/2-a)",
                NitelikliBolum.HizmetYeri => "Eğitim/sağlık hizmeti verilen yerde (TCK 142/2-b)",
                NitelikliBolum.MeskenIsyeri => "Mesken veya işyerinde (TCK 142/2-c)",
                _ => "Nitelikli hal"
            };
        }
    }
}
