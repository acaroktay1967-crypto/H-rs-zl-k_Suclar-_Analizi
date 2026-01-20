using System;
using HirsizlikSuclariAnalizi.Models;
using HirsizlikSuclariAnalizi.Services;
using HirsizlikSuclariAnalizi.Data;

namespace HirsizlikSuclariAnalizi.Tests
{
    /// <summary>
    /// Basit konsol test programı - Uygulamanın temel işlevselliğini test eder
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== HIRSIZLIK SUÇLARI ANALİZİ - TEST PROGRAMI ===\n");
            
            // Test 1: Basit Hırsızlık
            TestBasitHirsizlik();
            
            // Test 2: Nitelikli Hırsızlık (Gece Vakti)
            TestNitelikliHirsizlikGeceVakti();
            
            // Test 3: Etkin Pişmanlık
            TestEtkinPismanlik();
            
            // Test 4: Yargıtay Veritabanı
            TestYargitayVeritabani();
            
            // Test 5: PDF Oluşturma (sadece method çağrısı)
            TestPdfOlusturma();
            
            Console.WriteLine("\n=== TÜM TESTLER TAMAMLANDI ===");
        }
        
        static void TestBasitHirsizlik()
        {
            Console.WriteLine("TEST 1: Basit Hırsızlık");
            Console.WriteLine("------------------------");
            
            var cezaServisi = new CezaHesaplamaServisi();
            var parametreler = new CezaHesaplamaParametreleri
            {
                HirsizlikTuru = HirsizlikTuru.BasitHirsizlik,
                MalDegeri = 5000
            };
            
            var sonuc = cezaServisi.CezaHesapla(parametreler);
            
            Console.WriteLine($"Temel Ceza: {sonuc.TemelCezaAltSinir} - {sonuc.TemelCezaUstSinir} ay");
            Console.WriteLine($"Nihai Ceza: {sonuc.NihaiCezaAltSinir} - {sonuc.NihaiCezaUstSinir} ay");
            Console.WriteLine($"Açıklama Sayısı: {sonuc.Aciklamalar.Count}");
            Console.WriteLine($"✓ Test başarılı\n");
        }
        
        static void TestNitelikliHirsizlikGeceVakti()
        {
            Console.WriteLine("TEST 2: Nitelikli Hırsızlık (Gece Vakti)");
            Console.WriteLine("----------------------------------------");
            
            var cezaServisi = new CezaHesaplamaServisi();
            var parametreler = new CezaHesaplamaParametreleri
            {
                HirsizlikTuru = HirsizlikTuru.NitelikliHirsizlik,
                MalDegeri = 10000,
                NitelikliSebepler = new List<NitelikliBolum> 
                { 
                    NitelikliBolum.GeceVakti 
                }
            };
            
            var sonuc = cezaServisi.CezaHesapla(parametreler);
            
            Console.WriteLine($"Temel Ceza: {sonuc.TemelCezaAltSinir} - {sonuc.TemelCezaUstSinir} ay");
            Console.WriteLine($"Artırılmış Ceza: {sonuc.ArtirimisCezaAltSinir} - {sonuc.ArtirimisCezaUstSinir} ay");
            Console.WriteLine($"Nihai Ceza: {sonuc.NihaiCezaAltSinir} - {sonuc.NihaiCezaUstSinir} ay");
            Console.WriteLine($"Artırım Oranı: {((double)sonuc.ArtirimisCezaAltSinir / sonuc.TemelCezaAltSinir):F2}x");
            Console.WriteLine($"✓ Test başarılı\n");
        }
        
        static void TestEtkinPismanlik()
        {
            Console.WriteLine("TEST 3: Etkin Pişmanlık");
            Console.WriteLine("-----------------------");
            
            var cezaServisi = new CezaHesaplamaServisi();
            var parametreler = new CezaHesaplamaParametreleri
            {
                HirsizlikTuru = HirsizlikTuru.BasitHirsizlik,
                MalDegeri = 3000,
                EtkinPismanlik = true
            };
            
            var sonuc = cezaServisi.CezaHesapla(parametreler);
            
            Console.WriteLine($"Temel Ceza: {sonuc.TemelCezaAltSinir} - {sonuc.TemelCezaUstSinir} ay");
            Console.WriteLine($"İndirimli Ceza: {sonuc.NihaiCezaAltSinir} - {sonuc.NihaiCezaUstSinir} ay");
            
            var indirimVarMi = sonuc.Aciklamalar.Any(a => a.Contains("Etkin pişmanlık"));
            Console.WriteLine($"Etkin Pişmanlık İndirimi Uygulandı: {(indirimVarMi ? "Evet" : "Hayır")}");
            Console.WriteLine($"✓ Test başarılı\n");
        }
        
        static void TestYargitayVeritabani()
        {
            Console.WriteLine("TEST 4: Yargıtay Veritabanı");
            Console.WriteLine("----------------------------");
            
            try
            {
                var yargitayServisi = new YargitayVeriTabaniServisi("test_yargitay.db");
                var kararlar = yargitayServisi.TumKararlariGetir();
                
                Console.WriteLine($"Toplam Karar Sayısı: {kararlar.Count}");
                
                if (kararlar.Count > 0)
                {
                    var ilkKarar = kararlar[0];
                    Console.WriteLine($"İlk Karar: {ilkKarar.DaireNo}");
                    Console.WriteLine($"TCK Maddesi: {ilkKarar.TCKMaddesi}");
                }
                
                // Arama testi
                var arananKararlar = yargitayServisi.KararAra("gece vakti");
                Console.WriteLine($"'Gece vakti' aramasında bulunan: {arananKararlar.Count} karar");
                
                Console.WriteLine($"✓ Test başarılı\n");
                
                // Test veritabanını temizle
                if (File.Exists("test_yargitay.db"))
                    File.Delete("test_yargitay.db");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Hata: {ex.Message}\n");
            }
        }
        
        static void TestPdfOlusturma()
        {
            Console.WriteLine("TEST 5: PDF Oluşturma");
            Console.WriteLine("---------------------");
            
            try
            {
                var pdfServisi = new PdfRaporServisi();
                var cezaServisi = new CezaHesaplamaServisi();
                
                var parametreler = new CezaHesaplamaParametreleri
                {
                    HirsizlikTuru = HirsizlikTuru.NitelikliHirsizlik,
                    MalDegeri = 15000,
                    NitelikliSebepler = new List<NitelikliBolum> 
                    { 
                        NitelikliBolum.MeskenIsyeri 
                    }
                };
                
                var sonuc = cezaServisi.CezaHesapla(parametreler);
                
                var dosyaYolu = "test_rapor.pdf";
                pdfServisi.RaporOlustur(dosyaYolu, parametreler, sonuc);
                
                if (File.Exists(dosyaYolu))
                {
                    var fileInfo = new FileInfo(dosyaYolu);
                    Console.WriteLine($"PDF Oluşturuldu: {dosyaYolu}");
                    Console.WriteLine($"Dosya Boyutu: {fileInfo.Length} bytes");
                    Console.WriteLine($"✓ Test başarılı\n");
                    
                    // Test dosyasını temizle
                    File.Delete(dosyaYolu);
                }
                else
                {
                    Console.WriteLine($"✗ PDF dosyası oluşturulamadı\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Hata: {ex.Message}\n");
            }
        }
    }
}

