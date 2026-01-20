using System.Windows;
using HirsizlikSuclariAnalizi.Views;

namespace HirsizlikSuclariAnalizi;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        // Load default view
        BtnCezaHesaplama_Click(null, null);
    }

    private void BtnCezaHesaplama_Click(object? sender, RoutedEventArgs? e)
    {
        contentArea.Content = new CezaHesaplamaView();
    }

    private void BtnYargitayKararlari_Click(object sender, RoutedEventArgs e)
    {
        contentArea.Content = new YargitayKararlariView();
    }

    private void BtnHakkinda_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(
            "Hırsızlık Suçları Analiz Sistemi\n\n" +
            "Bu uygulama Türk Ceza Kanunu'nun 141 ve 142. maddelerine göre hırsızlık suçlarının analizini ve ceza hesaplamalarını yapmaktadır.\n\n" +
            "Özellikler:\n" +
            "• Basit ve nitelikli hırsızlık ayrımı\n" +
            "• Mal değeri, gece vakti ve etkin pişmanlık faktörlerine göre ceza hesaplama\n" +
            "• Yargıtay kararları inceleme ve filtreleme\n" +
            "• PDF raporlama\n\n" +
            "Versiyon: 1.0\n" +
            "2024",
            "Hakkında",
            MessageBoxButton.OK,
            MessageBoxImage.Information
        );
    }
}