using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using HirsizlikSuclariAnalizi.Models;
using HirsizlikSuclariAnalizi.Services;
using HirsizlikSuclariAnalizi.Data;
using Microsoft.Win32;

namespace HirsizlikSuclariAnalizi.ViewModels
{
    /// <summary>
    /// Ana pencere için ViewModel
    /// </summary>
    public partial class MainViewModel : ObservableObject
    {
        private readonly CezaHesaplamaServisi _cezaServisi;
        private readonly PdfRaporServisi _pdfServisi;
        private readonly YargitayVeriTabaniServisi _yargitayServisi;
        
        [ObservableProperty]
        private HirsizlikTuru _secilenHirsizlikTuru = HirsizlikTuru.BasitHirsizlik;
        
        [ObservableProperty]
        private decimal _malDegeri;
        
        [ObservableProperty]
        private bool _geceVakti;
        
        [ObservableProperty]
        private bool _birlikteSuc;
        
        [ObservableProperty]
        private bool _savunmasizMagdur;
        
        [ObservableProperty]
        private bool _kamuBinasi;
        
        [ObservableProperty]
        private bool _felaketDurumu;
        
        [ObservableProperty]
        private bool _diniYer;
        
        [ObservableProperty]
        private bool _hizmetYeri;
        
        [ObservableProperty]
        private bool _meskenIsyeri;
        
        [ObservableProperty]
        private bool _etkinPismanlik;
        
        [ObservableProperty]
        private bool _uzlastirma;
        
        [ObservableProperty]
        private bool _akrabaMagdur;
        
        [ObservableProperty]
        private bool _tesebbüs;
        
        [ObservableProperty]
        private bool _hagb;
        
        [ObservableProperty]
        private CezaSonucu? _hesaplananCeza;
        
        [ObservableProperty]
        private string _aramaMetni = string.Empty;
        
        [ObservableProperty]
        private YargitayKarari? _secilenKarar;
        
        public ObservableCollection<YargitayKarari> YargitayKararlari { get; } = new();
        
        public MainViewModel()
        {
            _cezaServisi = new CezaHesaplamaServisi();
            _pdfServisi = new PdfRaporServisi();
            _yargitayServisi = new YargitayVeriTabaniServisi();
            
            // Başlangıçta tüm kararları yükle
            YargitayKararlariYukle();
        }
        
        [RelayCommand]
        private void CezaHesapla()
        {
            try
            {
                var parametreler = new CezaHesaplamaParametreleri
                {
                    HirsizlikTuru = SecilenHirsizlikTuru,
                    MalDegeri = MalDegeri,
                    EtkinPismanlik = EtkinPismanlik,
                    Uzlastirma = Uzlastirma,
                    AkrabaMagdur = AkrabaMagdur,
                    Tesebbüs = Tesebbüs,
                    HukmunAciklanmasininGeribirakılmasi = Hagb
                };
                
                // Nitelikli sebepleri ekle
                if (GeceVakti) parametreler.NitelikliSebepler.Add(NitelikliBolum.GeceVakti);
                if (BirlikteSuc) parametreler.NitelikliSebepler.Add(NitelikliBolum.BirlikteSuc);
                if (SavunmasizMagdur) parametreler.NitelikliSebepler.Add(NitelikliBolum.SavunmasizMagdur);
                if (KamuBinasi) parametreler.NitelikliSebepler.Add(NitelikliBolum.KamuBinasi);
                if (FelaketDurumu) parametreler.NitelikliSebepler.Add(NitelikliBolum.FelaketDurumu);
                if (DiniYer) parametreler.NitelikliSebepler.Add(NitelikliBolum.DiniYer);
                if (HizmetYeri) parametreler.NitelikliSebepler.Add(NitelikliBolum.HizmetYeri);
                if (MeskenIsyeri) parametreler.NitelikliSebepler.Add(NitelikliBolum.MeskenIsyeri);
                
                HesaplananCeza = _cezaServisi.CezaHesapla(parametreler);
                
                MessageBox.Show("Ceza hesaplama tamamlandı! Sonuçlar aşağıda görüntüleniyor.", 
                    "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ceza hesaplama sırasında hata oluştu: {ex.Message}", 
                    "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        [RelayCommand]
        private void RaporOlustur()
        {
            if (HesaplananCeza == null)
            {
                MessageBox.Show("Önce ceza hesaplaması yapmalısınız!", 
                    "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            try
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = "PDF Dosyası|*.pdf",
                    FileName = $"Hirsizlik_Ceza_Raporu_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                    DefaultExt = "pdf"
                };
                
                if (saveDialog.ShowDialog() == true)
                {
                    var parametreler = new CezaHesaplamaParametreleri
                    {
                        HirsizlikTuru = SecilenHirsizlikTuru,
                        MalDegeri = MalDegeri,
                        EtkinPismanlik = EtkinPismanlik,
                        Uzlastirma = Uzlastirma,
                        AkrabaMagdur = AkrabaMagdur,
                        Tesebbüs = Tesebbüs,
                        HukmunAciklanmasininGeribirakılmasi = Hagb
                    };
                    
                    if (GeceVakti) parametreler.NitelikliSebepler.Add(NitelikliBolum.GeceVakti);
                    if (BirlikteSuc) parametreler.NitelikliSebepler.Add(NitelikliBolum.BirlikteSuc);
                    if (SavunmasizMagdur) parametreler.NitelikliSebepler.Add(NitelikliBolum.SavunmasizMagdur);
                    if (KamuBinasi) parametreler.NitelikliSebepler.Add(NitelikliBolum.KamuBinasi);
                    if (FelaketDurumu) parametreler.NitelikliSebepler.Add(NitelikliBolum.FelaketDurumu);
                    if (DiniYer) parametreler.NitelikliSebepler.Add(NitelikliBolum.DiniYer);
                    if (HizmetYeri) parametreler.NitelikliSebepler.Add(NitelikliBolum.HizmetYeri);
                    if (MeskenIsyeri) parametreler.NitelikliSebepler.Add(NitelikliBolum.MeskenIsyeri);
                    
                    _pdfServisi.RaporOlustur(saveDialog.FileName, parametreler, HesaplananCeza);
                    
                    MessageBox.Show($"Rapor başarıyla oluşturuldu:\n{saveDialog.FileName}", 
                        "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Rapor oluşturma sırasında hata oluştu: {ex.Message}", 
                    "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        [RelayCommand]
        private void YargitayAra()
        {
            try
            {
                YargitayKararlari.Clear();
                
                var kararlar = string.IsNullOrWhiteSpace(AramaMetni)
                    ? _yargitayServisi.TumKararlariGetir()
                    : _yargitayServisi.KararAra(AramaMetni);
                
                foreach (var karar in kararlar)
                {
                    YargitayKararlari.Add(karar);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Arama sırasında hata oluştu: {ex.Message}", 
                    "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        [RelayCommand]
        private void Temizle()
        {
            MalDegeri = 0;
            GeceVakti = false;
            BirlikteSuc = false;
            SavunmasizMagdur = false;
            KamuBinasi = false;
            FelaketDurumu = false;
            DiniYer = false;
            HizmetYeri = false;
            MeskenIsyeri = false;
            EtkinPismanlik = false;
            Uzlastirma = false;
            AkrabaMagdur = false;
            Tesebbüs = false;
            Hagb = false;
            HesaplananCeza = null;
        }
        
        private void YargitayKararlariYukle()
        {
            try
            {
                var kararlar = _yargitayServisi.TumKararlariGetir();
                foreach (var karar in kararlar)
                {
                    YargitayKararlari.Add(karar);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yargıtay kararları yüklenirken hata oluştu: {ex.Message}", 
                    "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
