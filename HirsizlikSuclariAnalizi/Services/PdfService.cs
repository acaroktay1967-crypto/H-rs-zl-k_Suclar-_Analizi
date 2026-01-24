using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using HirsizlikSuclariAnalizi.Models;

namespace HirsizlikSuclariAnalizi.Services
{
    public class PdfService
    {
        public void GenerateCezaRaporu(CezaHesaplama ceza, string filePath)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text("Hırsızlık Suçu Ceza Hesaplama Raporu")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(15);

                            x.Item().Text($"Rapor Tarihi: {ceza.HesaplamaTarihi:dd.MM.yyyy HH:mm}");
                            
                            x.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                            x.Item().Text("Suç Bilgileri").SemiBold().FontSize(16);
                            x.Item().Text($"Hırsızlık Türü: {GetHirsizlikTuruText(ceza.HirsizlikTuru)}");
                            x.Item().Text($"Mal Değeri: {ceza.MalDegeri:C}");
                            x.Item().Text($"Gece Vakti: {(ceza.GeceVakti ? "Evet" : "Hayır")}");
                            x.Item().Text($"Etkin Pişmanlık: {(ceza.EtkinPismanlik ? "Evet" : "Hayır")}");

                            x.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                            x.Item().Text("Ceza Hesaplaması").SemiBold().FontSize(16);
                            x.Item().Text($"Minimum Ceza: {ceza.MinCeza} yıl");
                            x.Item().Text($"Maximum Ceza: {ceza.MaxCeza} yıl");

                            if (!string.IsNullOrWhiteSpace(ceza.Notlar))
                            {
                                x.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                                x.Item().Text("Notlar").SemiBold().FontSize(16);
                                x.Item().Text(ceza.Notlar);
                            }

                            x.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                            x.Item().Text("Yasal Dayanak").SemiBold().FontSize(16);
                            if (ceza.HirsizlikTuru == HirsizlikTuru.BasitHirsizlik)
                            {
                                x.Item().Text("TCK 141. Madde - Basit Hırsızlık").FontSize(10);
                                x.Item().Text("Başkasına ait taşınır bir malı, zilyedinin rızası olmaksızın kendisine veya başkasına bir yarar sağlamak maksadıyla bulunduğu yerden alan kimseye bir yıldan üç yıla kadar hapis cezası verilir.").FontSize(10);
                            }
                            else
                            {
                                x.Item().Text("TCK 142. Madde - Nitelikli Hırsızlık").FontSize(10);
                                x.Item().Text("Hırsızlık suçunun belirli hallerde işlenmesi durumunda ceza yarı oranında artırılır.").FontSize(10);
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Sayfa ");
                            x.CurrentPageNumber();
                        });
                });
            })
            .GeneratePdf(filePath);
        }

        public void GenerateYargitayKarariRaporu(YargitayKarari karar, string filePath)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text("Yargıtay Kararı Raporu")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(15);

                            x.Item().Text($"Karar Numarası: {karar.KararNumarasi}").SemiBold().FontSize(14);
                            x.Item().Text($"Karar Tarihi: {karar.KararTarihi:dd.MM.yyyy}");
                            x.Item().Text($"Suç Türü: {GetHirsizlikTuruText(karar.SucTuru)}");

                            x.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                            x.Item().Text("Özet").SemiBold().FontSize(14);
                            x.Item().Text(karar.Ozet);

                            x.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                            x.Item().Text("Tam Metin").SemiBold().FontSize(14);
                            x.Item().Text(karar.TamMetin);
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Sayfa ");
                            x.CurrentPageNumber();
                        });
                });
            })
            .GeneratePdf(filePath);
        }

        private string GetHirsizlikTuruText(HirsizlikTuru tur)
        {
            return tur == HirsizlikTuru.BasitHirsizlik ? "Basit Hırsızlık" : "Nitelikli Hırsızlık";
        }
    }
}
