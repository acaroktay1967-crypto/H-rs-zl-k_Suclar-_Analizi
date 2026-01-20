using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using HirsizlikSuclariAnalizi.Models;
using HirsizlikSuclariAnalizi.Data;
using HirsizlikSuclariAnalizi.Services;

namespace HirsizlikSuclariAnalizi.Views
{
    public partial class YargitayKararlariView : UserControl
    {
        private readonly DatabaseService _dbService;
        private readonly PdfService _pdfService;

        public YargitayKararlariView()
        {
            InitializeComponent();
            _dbService = new DatabaseService();
            _pdfService = new PdfService();
            LoadKararlar();
        }

        private void LoadKararlar(string? kararNumarasi = null, HirsizlikTuru? sucTuru = null)
        {
            try
            {
                var kararlar = _dbService.GetYargitayKararlari(kararNumarasi, sucTuru);
                
                // SucTuruText property ekle
                var kararlarWithText = kararlar.Select(k => new
                {
                    k.Id,
                    k.KararNumarasi,
                    k.KararTarihi,
                    SucTuruText = k.SucTuru == HirsizlikTuru.BasitHirsizlik ? "Basit Hırsızlık" : "Nitelikli Hırsızlık",
                    k.Ozet,
                    k.TamMetin,
                    OriginalKarar = k
                }).ToList();

                dgKararlar.ItemsSource = kararlarWithText;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kararlar yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnFiltrele_Click(object sender, RoutedEventArgs e)
        {
            HirsizlikTuru? sucTuru = null;
            
            if (cmbSucTuru.SelectedIndex == 1) // Basit Hırsızlık
                sucTuru = HirsizlikTuru.BasitHirsizlik;
            else if (cmbSucTuru.SelectedIndex == 2) // Nitelikli Hırsızlık
                sucTuru = HirsizlikTuru.NitelikliHirsizlik;

            var kararNumarasi = string.IsNullOrWhiteSpace(txtKararNumarasi.Text) ? null : txtKararNumarasi.Text;
            
            LoadKararlar(kararNumarasi, sucTuru);
        }

        private void DgKararlar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgKararlar.SelectedItem != null)
            {
                dynamic selectedItem = dgKararlar.SelectedItem;
                YargitayKarari karar = selectedItem.OriginalKarar;
                
                var detayWindow = new Window
                {
                    Title = $"Karar Detayı - {karar.KararNumarasi}",
                    Width = 600,
                    Height = 500,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                var grid = new Grid { Margin = new Thickness(20) };
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                var headerStack = new StackPanel { Margin = new Thickness(0, 0, 0, 15) };
                headerStack.Children.Add(new TextBlock
                {
                    Text = $"Karar Numarası: {karar.KararNumarasi}",
                    FontSize = 16,
                    FontWeight = FontWeights.Bold
                });
                headerStack.Children.Add(new TextBlock
                {
                    Text = $"Tarih: {karar.KararTarihi:dd.MM.yyyy}",
                    FontSize = 12,
                    Margin = new Thickness(0, 5, 0, 0)
                });
                headerStack.Children.Add(new TextBlock
                {
                    Text = $"Suç Türü: {(karar.SucTuru == HirsizlikTuru.BasitHirsizlik ? "Basit Hırsızlık" : "Nitelikli Hırsızlık")}",
                    FontSize = 12,
                    Margin = new Thickness(0, 5, 0, 0)
                });
                Grid.SetRow(headerStack, 0);
                grid.Children.Add(headerStack);

                var scrollViewer = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
                var contentStack = new StackPanel();
                contentStack.Children.Add(new TextBlock
                {
                    Text = "Özet:",
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 5)
                });
                contentStack.Children.Add(new TextBlock
                {
                    Text = karar.Ozet,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 0, 0, 15)
                });
                contentStack.Children.Add(new TextBlock
                {
                    Text = "Tam Metin:",
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 5)
                });
                contentStack.Children.Add(new TextBlock
                {
                    Text = karar.TamMetin,
                    TextWrapping = TextWrapping.Wrap
                });
                scrollViewer.Content = contentStack;
                Grid.SetRow(scrollViewer, 1);
                grid.Children.Add(scrollViewer);

                var btnPdf = new Button
                {
                    Content = "PDF Olarak Kaydet",
                    Padding = new Thickness(15, 8, 15, 8),
                    Margin = new Thickness(0, 15, 0, 0),
                    Background = System.Windows.Media.Brushes.DarkRed,
                    Foreground = System.Windows.Media.Brushes.White,
                    Cursor = System.Windows.Input.Cursors.Hand
                };
                btnPdf.Click += (s, args) =>
                {
                    try
                    {
                        var saveDialog = new SaveFileDialog
                        {
                            Filter = "PDF dosyaları (*.pdf)|*.pdf",
                            FileName = $"Yargitay_Karar_{karar.KararNumarasi.Replace("/", "_")}_{DateTime.Now:yyyyMMdd}.pdf"
                        };

                        if (saveDialog.ShowDialog() == true)
                        {
                            _pdfService.GenerateYargitayKarariRaporu(karar, saveDialog.FileName);
                            MessageBox.Show("PDF raporu başarıyla oluşturuldu.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                            
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = saveDialog.FileName,
                                UseShellExecute = true
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"PDF oluşturma sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                };
                Grid.SetRow(btnPdf, 2);
                grid.Children.Add(btnPdf);

                detayWindow.Content = grid;
                detayWindow.ShowDialog();
            }
        }
    }
}
