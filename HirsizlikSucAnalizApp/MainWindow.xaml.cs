using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace HirsizlikSucAnalizApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Kanun dosyasını okuyup RichTextBox'a yükler
    /// </summary>
    private void LoadKanunText(string fileName)
    {
        try
        {
            // Kanun dosyasının yolunu belirle
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = System.IO.Path.Combine(baseDirectory, "kanunlar", fileName);

            // Dosya var mı kontrol et
            if (!File.Exists(filePath))
            {
                MessageBox.Show($"Kanun dosyası bulunamadı: {fileName}", 
                    "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Dosyayı oku
            string kanunMetni = File.ReadAllText(filePath, Encoding.UTF8);

            // RichTextBox'ı temizle ve yeni metni yükle
            rtbKanunMetni.Document.Blocks.Clear();
            
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run(kanunMetni));
            rtbKanunMetni.Document.Blocks.Add(paragraph);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Kanun dosyası okunurken hata oluştu: {ex.Message}", 
                "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// TCK Butonu tıklama olayı
    /// </summary>
    private void BtnTCK_Click(object sender, RoutedEventArgs e)
    {
        LoadKanunText("TCK.txt");
    }

    /// <summary>
    /// CMK Butonu tıklama olayı
    /// </summary>
    private void BtnCMK_Click(object sender, RoutedEventArgs e)
    {
        LoadKanunText("CMK.txt");
    }

    /// <summary>
    /// TMK Butonu tıklama olayı
    /// </summary>
    private void BtnTMK_Click(object sender, RoutedEventArgs e)
    {
        LoadKanunText("TMK.txt");
    }

    /// <summary>
    /// TBK Butonu tıklama olayı
    /// </summary>
    private void BtnTBK_Click(object sender, RoutedEventArgs e)
    {
        LoadKanunText("TBK.txt");
    }

    /// <summary>
    /// ÇKK Butonu tıklama olayı
    /// </summary>
    private void BtnCIK_Click(object sender, RoutedEventArgs e)
    {
        LoadKanunText("CIK.txt");
    }

    /// <summary>
    /// Temizle butonu tıklama olayı
    /// </summary>
    private void BtnTemizle_Click(object sender, RoutedEventArgs e)
    {
        rtbKanunMetni.Document.Blocks.Clear();
        
        Paragraph paragraph = new Paragraph();
        Run run = new Run("Kanun görüntülemek için yukarıdaki butonlardan birini seçin.")
        {
            FontStyle = FontStyles.Italic,
            Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102))
        };
        paragraph.Inlines.Add(run);
        rtbKanunMetni.Document.Blocks.Add(paragraph);
    }
}