namespace HirsizlikSuclariAnalizi.Models
{
    public class CezaHesaplama
    {
        public int Id { get; set; }
        public HirsizlikTuru HirsizlikTuru { get; set; }
        public decimal MalDegeri { get; set; }
        public bool GeceVakti { get; set; }
        public bool EtkinPismanlik { get; set; }
        public double MinCeza { get; set; }
        public double MaxCeza { get; set; }
        public DateTime HesaplamaTarihi { get; set; }
        public string? Notlar { get; set; }

        public CezaHesaplama()
        {
            HesaplamaTarihi = DateTime.Now;
        }
    }
}
