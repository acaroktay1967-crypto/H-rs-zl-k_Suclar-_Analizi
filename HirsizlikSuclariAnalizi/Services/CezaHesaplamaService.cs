using HirsizlikSuclariAnalizi.Models;

namespace HirsizlikSuclariAnalizi.Services
{
    public class CezaHesaplamaService
    {
        // TCK 141. Madde: Basit Hırsızlık - 1 yıldan 3 yıla kadar hapis cezası
        private const double BasitHirsizlikMinCeza = 1.0;
        private const double BasitHirsizlikMaxCeza = 3.0;

        // TCK 142. Madde: Nitelikli Hırsızlık - Ceza yarı oranında artırılır
        private const double NitelikliHirsizlikArtirimOrani = 1.5;

        // Etkin Pişmanlık - Ceza üçte birine kadar indirilebilir
        private const double EtkinPismanlikIndirimOrani = 0.67;

        public (double minCeza, double maxCeza) HesaplaCeza(
            HirsizlikTuru hirsizlikTuru,
            decimal malDegeri,
            bool geceVakti,
            bool etkinPismanlik)
        {
            double minCeza, maxCeza;

            if (hirsizlikTuru == HirsizlikTuru.BasitHirsizlik)
            {
                // Basit hırsızlık için temel ceza
                minCeza = BasitHirsizlikMinCeza;
                maxCeza = BasitHirsizlikMaxCeza;

                // Mal değerine göre düzeltme
                if (malDegeri < 1000)
                {
                    // Düşük değerde ceza alt sınırdan uygulanır
                    maxCeza = minCeza + (maxCeza - minCeza) * 0.3;
                }
                else if (malDegeri > 10000)
                {
                    // Yüksek değerde ceza üst sınıra yakın uygulanır
                    minCeza = minCeza + (maxCeza - minCeza) * 0.6;
                }
            }
            else
            {
                // Nitelikli hırsızlık veya gece vakti işlenen hırsızlık
                minCeza = BasitHirsizlikMinCeza * NitelikliHirsizlikArtirimOrani;
                maxCeza = BasitHirsizlikMaxCeza * NitelikliHirsizlikArtirimOrani;

                // Mal değerine göre düzeltme
                if (malDegeri < 1000)
                {
                    maxCeza = minCeza + (maxCeza - minCeza) * 0.3;
                }
                else if (malDegeri > 10000)
                {
                    minCeza = minCeza + (maxCeza - minCeza) * 0.6;
                }
            }

            // Gece vakti ek artırım (nitelikli hırsızlık değilse)
            if (geceVakti && hirsizlikTuru == HirsizlikTuru.BasitHirsizlik)
            {
                minCeza *= NitelikliHirsizlikArtirimOrani;
                maxCeza *= NitelikliHirsizlikArtirimOrani;
            }

            // Etkin pişmanlık indirimi
            if (etkinPismanlik)
            {
                minCeza *= EtkinPismanlikIndirimOrani;
                maxCeza *= EtkinPismanlikIndirimOrani;
            }

            return (Math.Round(minCeza, 2), Math.Round(maxCeza, 2));
        }

        public string GetCezaAciklamasi(
            HirsizlikTuru hirsizlikTuru,
            bool geceVakti,
            bool etkinPismanlik)
        {
            var aciklama = new System.Text.StringBuilder();

            if (hirsizlikTuru == HirsizlikTuru.BasitHirsizlik)
            {
                aciklama.AppendLine("TCK 141. Madde - Basit Hırsızlık");
                aciklama.AppendLine("Temel Ceza: 1 yıldan 3 yıla kadar hapis");
            }
            else
            {
                aciklama.AppendLine("TCK 142. Madde - Nitelikli Hırsızlık");
                aciklama.AppendLine("Temel Ceza: Basit hırsızlık cezası yarı oranında artırılır");
            }

            if (geceVakti)
            {
                aciklama.AppendLine("\nGece vakti işlenme: Ceza yarı oranında artırılır");
            }

            if (etkinPismanlik)
            {
                aciklama.AppendLine("\nEtkin Pişmanlık: Ceza üçte birine kadar indirilebilir");
            }

            return aciklama.ToString();
        }
    }
}
