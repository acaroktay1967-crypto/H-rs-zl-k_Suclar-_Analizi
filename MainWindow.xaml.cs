using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace TextEditor
{
    public partial class MainWindow : Window
    {
        private bool isHighlighting = false;

        public MainWindow()
        {
            InitializeComponent();
            InitializeFontComboBox();
            InitializeFontSizeComboBox();
        }

        private void InitializeFontComboBox()
        {
            var fonts = Fonts.SystemFontFamilies.OrderBy(f => f.Source).ToList();
            FontFamilyComboBox.ItemsSource = fonts.Select(f => f.Source);
            FontFamilyComboBox.SelectedItem = "Arial";
        }

        private void InitializeFontSizeComboBox()
        {
            var sizes = new List<double> { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            FontSizeComboBox.ItemsSource = sizes;
            FontSizeComboBox.SelectedItem = 12.0;
        }

        #region Unicode Highlighting

        private void MainTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isHighlighting)
            {
                UpdateCharacterCount();
                HighlightUnicodeCharacters();
            }
        }

        private void UpdateCharacterCount()
        {
            TextRange textRange = new TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd);
            string text = textRange.Text;
            int charCount = text.Length - 1; // Remove extra newline
            CharCountText.Text = $"Karakter: {Math.Max(0, charCount)}";
        }

        private void HighlightUnicodeCharacters()
        {
            isHighlighting = true;
            
            try
            {
                TextPointer start = MainTextBox.Document.ContentStart;
                TextPointer end = MainTextBox.Document.ContentEnd;
                
                // Reset all formatting first
                TextRange fullRange = new TextRange(start, end);
                
                // Get current text
                string text = fullRange.Text;
                
                TextPointer current = start;
                
                while (current != null && current.CompareTo(end) < 0)
                {
                    if (current.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                    {
                        string textRun = current.GetTextInRun(LogicalDirection.Forward);
                        
                        for (int i = 0; i < textRun.Length; i++)
                        {
                            char c = textRun[i];
                            
                            // Check if character is a special Unicode character
                            if (IsSpecialUnicodeCharacter(c))
                            {
                                TextPointer charStart = current.GetPositionAtOffset(i);
                                TextPointer charEnd = current.GetPositionAtOffset(i + 1);
                                
                                if (charStart != null && charEnd != null)
                                {
                                    TextRange charRange = new TextRange(charStart, charEnd);
                                    charRange.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
                                }
                            }
                        }
                    }
                    
                    current = current.GetNextContextPosition(LogicalDirection.Forward);
                }
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Hata: {ex.Message}";
            }
            finally
            {
                isHighlighting = false;
            }
        }

        private bool IsSpecialUnicodeCharacter(char c)
        {
            // Check for various Unicode character categories
            int code = (int)c;
            
            // Special symbols, mathematical operators, arrows, box drawing, etc.
            return (code >= 0x0080 && code <= 0x024F) || // Latin Extended
                   (code >= 0x0370 && code <= 0x03FF) || // Greek
                   (code >= 0x0400 && code <= 0x04FF) || // Cyrillic
                   (code >= 0x0530 && code <= 0x058F) || // Armenian
                   (code >= 0x0590 && code <= 0x05FF) || // Hebrew
                   (code >= 0x0600 && code <= 0x06FF) || // Arabic
                   (code >= 0x2000 && code <= 0x206F) || // General Punctuation
                   (code >= 0x2070 && code <= 0x209F) || // Superscripts and Subscripts
                   (code >= 0x20A0 && code <= 0x20CF) || // Currency Symbols
                   (code >= 0x2100 && code <= 0x214F) || // Letterlike Symbols
                   (code >= 0x2190 && code <= 0x21FF) || // Arrows
                   (code >= 0x2200 && code <= 0x22FF) || // Mathematical Operators
                   (code >= 0x2300 && code <= 0x23FF) || // Miscellaneous Technical
                   (code >= 0x2500 && code <= 0x257F) || // Box Drawing
                   (code >= 0x2580 && code <= 0x259F) || // Block Elements
                   (code >= 0x25A0 && code <= 0x25FF) || // Geometric Shapes
                   (code >= 0x2600 && code <= 0x26FF) || // Miscellaneous Symbols
                   (code >= 0x2700 && code <= 0x27BF);   // Dingbats
        }

        #endregion

        #region File Operations

        private void NewDocument_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Mevcut belgeyi kaydetmeden yeni belge oluşturmak istiyor musunuz?",
                "Yeni Belge", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                MainTextBox.Document.Blocks.Clear();
                MainTextBox.Document.Blocks.Add(new Paragraph(new Run("")));
                StatusText.Text = "Yeni belge oluşturuldu";
            }
        }

        private void OpenDocument_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "RTF Dosyaları (*.rtf)|*.rtf|Tüm Dosyalar (*.*)|*.*",
                Title = "Belge Aç"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    TextRange range = new TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd);
                    using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open))
                    {
                        range.Load(fileStream, DataFormats.Rtf);
                    }
                    StatusText.Text = $"Belge açıldı: {Path.GetFileName(openFileDialog.FileName)}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Belge açılırken hata oluştu: {ex.Message}", "Hata", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveDocument_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "RTF Dosyaları (*.rtf)|*.rtf|Tüm Dosyalar (*.*)|*.*",
                Title = "Belge Kaydet",
                DefaultExt = "rtf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    TextRange range = new TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd);
                    using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        range.Save(fileStream, DataFormats.Rtf);
                    }
                    StatusText.Text = $"Belge kaydedildi: {Path.GetFileName(saveFileDialog.FileName)}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Belge kaydedilirken hata oluştu: {ex.Message}", "Hata", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion

        #region Export Functions

        private void ExportToWord_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Belgesi (*.docx)|*.docx",
                Title = "Word'e Aktar",
                DefaultExt = "docx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    ExportToWordDocument(saveFileDialog.FileName);
                    StatusText.Text = $"Word belgesine aktarıldı: {Path.GetFileName(saveFileDialog.FileName)}";
                    MessageBox.Show("Belge başarıyla Word formatına aktarıldı.", "Başarılı", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Word'e aktarma sırasında hata oluştu: {ex.Message}", "Hata", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExportToWordDocument(string fileName)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                TextRange textRange = new TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd);
                string text = textRange.Text;

                var lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                foreach (var line in lines)
                {
                    DocumentFormat.OpenXml.Wordprocessing.Paragraph para = body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                    DocumentFormat.OpenXml.Wordprocessing.Run run = para.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                    run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(line));
                }

                mainPart.Document.Save();
            }
        }

        private void ExportToPDF_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF Dosyası (*.pdf)|*.pdf",
                Title = "PDF'e Aktar",
                DefaultExt = "pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    ExportToPdfDocument(saveFileDialog.FileName);
                    StatusText.Text = $"PDF'e aktarıldı: {Path.GetFileName(saveFileDialog.FileName)}";
                    MessageBox.Show("Belge başarıyla PDF formatına aktarıldı.", "Başarılı", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"PDF'e aktarma sırasında hata oluştu: {ex.Message}", "Hata", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExportToPdfDocument(string fileName)
        {
            Document pdfDoc = new Document(PageSize.A4);
            PdfWriter.GetInstance(pdfDoc, new FileStream(fileName, FileMode.Create));
            pdfDoc.Open();

            // Create font with Unicode support
            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
            BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font font = new Font(baseFont, 12);

            TextRange textRange = new TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd);
            string text = textRange.Text;

            var lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                pdfDoc.Add(new iTextSharp.text.Paragraph(line, font));
            }

            pdfDoc.Close();
        }

        private void ExportToUYAP_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "UYAP UDF Dosyası (*.udf)|*.udf",
                Title = "UYAP UDF'e Aktar",
                DefaultExt = "udf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    ExportToUYAPFormat(saveFileDialog.FileName);
                    StatusText.Text = $"UYAP UDF'e aktarıldı: {Path.GetFileName(saveFileDialog.FileName)}";
                    MessageBox.Show("Belge başarıyla UYAP UDF formatına aktarıldı.", "Başarılı", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"UYAP UDF'e aktarma sırasında hata oluştu: {ex.Message}", "Hata", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExportToUYAPFormat(string fileName)
        {
            // UYAP UDF format is essentially an XML-based format
            TextRange textRange = new TextRange(MainTextBox.Document.ContentStart, MainTextBox.Document.ContentEnd);
            string text = textRange.Text;

            using (StreamWriter writer = new StreamWriter(fileName, false, System.Text.Encoding.UTF8))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                writer.WriteLine("<UYAPBelge>");
                writer.WriteLine("  <Baslik>UYAP Belge</Baslik>");
                writer.WriteLine($"  <Tarih>{DateTime.Now:yyyy-MM-dd HH:mm:ss}</Tarih>");
                writer.WriteLine("  <Icerik>");
                
                var lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                foreach (var line in lines)
                {
                    string escapedLine = System.Security.SecurityElement.Escape(line);
                    writer.WriteLine($"    <Paragraf>{escapedLine}</Paragraf>");
                }
                
                writer.WriteLine("  </Icerik>");
                writer.WriteLine("</UYAPBelge>");
            }
        }

        #endregion

        #region Edit Operations

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            MainTextBox.Cut();
            StatusText.Text = "Metin kesildi";
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            MainTextBox.Copy();
            StatusText.Text = "Metin kopyalandı";
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            MainTextBox.Paste();
            StatusText.Text = "Metin yapıştırıldı";
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            MainTextBox.SelectAll();
            StatusText.Text = "Tümü seçildi";
        }

        #endregion

        #region Formatting Operations

        private void Bold_Click(object sender, RoutedEventArgs e)
        {
            ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold, FontWeights.Normal);
        }

        private void Italic_Click(object sender, RoutedEventArgs e)
        {
            ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic, FontStyles.Normal);
        }

        private void Underline_Click(object sender, RoutedEventArgs e)
        {
            TextDecorationCollection underline = TextDecorations.Underline;
            ApplyPropertyValue(Inline.TextDecorationsProperty, underline, null);
        }

        private void ApplyPropertyValue(DependencyProperty property, object value, object? defaultValue)
        {
            if (MainTextBox.Selection != null && !MainTextBox.Selection.IsEmpty)
            {
                object currentValue = MainTextBox.Selection.GetPropertyValue(property);
                
                if (currentValue != null && currentValue.Equals(value))
                {
                    MainTextBox.Selection.ApplyPropertyValue(property, defaultValue);
                }
                else
                {
                    MainTextBox.Selection.ApplyPropertyValue(property, value);
                }
            }
        }

        private void FontFamily_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (FontFamilyComboBox.SelectedItem != null && MainTextBox != null)
            {
                string fontFamily = FontFamilyComboBox.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(fontFamily))
                {
                    if (MainTextBox.Selection != null && !MainTextBox.Selection.IsEmpty)
                    {
                        MainTextBox.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, new FontFamily(fontFamily));
                    }
                }
            }
        }

        private void FontSize_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (FontSizeComboBox.SelectedItem != null && MainTextBox != null)
            {
                double fontSize = (double)FontSizeComboBox.SelectedItem;
                if (MainTextBox.Selection != null && !MainTextBox.Selection.IsEmpty)
                {
                    MainTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, fontSize);
                }
            }
        }

        #endregion

        #region Print Function

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    FlowDocument flowDoc = MainTextBox.Document;
                    
                    // Store the original document paginator
                    IDocumentPaginatorSource idocument = flowDoc as IDocumentPaginatorSource;
                    
                    printDialog.PrintDocument(idocument.DocumentPaginator, "Metin Editörü - Yazdırma");
                    StatusText.Text = "Belge yazdırıldı";
                    MessageBox.Show("Belge başarıyla yazdırıldı.", "Yazdırma", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yazdırma sırasında hata oluştu: {ex.Message}", "Hata", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Other

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Metin Editörü v1.0\n\n" +
                          "Özellikler:\n" +
                          "- Unicode karakter vurgulama\n" +
                          "- Temel metin düzenleme araçları\n" +
                          "- Word, PDF ve UYAP UDF formatına aktarma\n" +
                          "- Yazdırma desteği\n\n" +
                          "© 2024", 
                          "Hakkında", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Uygulamadan çıkmak istediğinizden emin misiniz?",
                "Çıkış", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        #endregion
    }
}
