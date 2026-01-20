using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using HirsizlikSuclariAnalizi.Models;
using HirsizlikSuclariAnalizi.Services;
using HirsizlikSuclariAnalizi.Data;

namespace HirsizlikSuclariAnalizi.Views
{
    public partial class CezaHesaplamaView : UserControl
    {
        private readonly CezaHesaplamaService _cezaService;
        private readonly PdfService _pdfService;
        private readonly DatabaseService _dbService;
        private CezaHesaplama? _sonCezaHesaplama;

        public CezaHesaplamaView()
        {
            InitializeComponent();
            _cezaService = new CezaHesaplamaService();
            _pdfService = new PdfService();
            _dbService = new DatabaseService();
        }

        private void BtnHesapla_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Mal değerini al
                if (!decimal.TryParse(txtMalDegeri.Text, out decimal malDegeri) || malDegeri <= 0)
                {
                    MessageBox.Show("Lütfen geçerli bir mal değeri giriniz.", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Hırsızlık türünü belirle
                var hirsizlikTuru = rbBasitHirsizlik.IsChecked == true 
                    ? HirsizlikTuru.BasitHirsizlik 
                    : HirsizlikTuru.NitelikliHirsizlik;

                var geceVakti = cbGeceVakti.IsChecked == true;
                var etkinPismanlik = cbEtkinPismanlik.IsChecked == true;

                // Ceza hesapla
                var (minCeza, maxCeza) = _cezaService.HesaplaCeza(hirsizlikTuru, malDegeri, geceVakti, etkinPismanlik);
                var aciklama = _cezaService.GetCezaAciklamasi(hirsizlikTuru, geceVakti, etkinPismanlik);

                // Sonuçları sakla
                _sonCezaHesaplama = new CezaHesaplama
                {
                    HirsizlikTuru = hirsizlikTuru,
                    MalDegeri = malDegeri,
                    GeceVakti = geceVakti,
                    EtkinPismanlik = etkinPismanlik,
                    MinCeza = minCeza,
                    MaxCeza = maxCeza,
                    Notlar = txtNotlar.Text
                };

                // Sonuçları göster
                txtSonuc.Text = $"Hesaplanan Ceza: {minCeza} yıl - {maxCeza} yıl hapis";
                txtAciklama.Text = aciklama;
                gbSonuc.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hesaplama sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnKaydet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_sonCezaHesaplama != null)
                {
                    _dbService.SaveCezaHesaplama(_sonCezaHesaplama);
                    MessageBox.Show("Hesaplama başarıyla kaydedildi.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kaydetme sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnPdfOlustur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_sonCezaHesaplama != null)
                {
                    var saveDialog = new SaveFileDialog
                    {
                        Filter = "PDF dosyaları (*.pdf)|*.pdf",
                        FileName = $"Ceza_Raporu_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                    };

                    if (saveDialog.ShowDialog() == true)
                    {
                        _pdfService.GenerateCezaRaporu(_sonCezaHesaplama, saveDialog.FileName);
                        MessageBox.Show("PDF raporu başarıyla oluşturuldu.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                        
                        // PDF'i aç
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = saveDialog.FileName,
                            UseShellExecute = true
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"PDF oluşturma sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
