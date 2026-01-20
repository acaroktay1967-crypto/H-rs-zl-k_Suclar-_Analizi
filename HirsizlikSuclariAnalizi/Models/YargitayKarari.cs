namespace HirsizlikSuclariAnalizi.Models
{
    public class YargitayKarari
    {
        public int Id { get; set; }
        public string KararNumarasi { get; set; } = string.Empty;
        public HirsizlikTuru SucTuru { get; set; }
        public DateTime KararTarihi { get; set; }
        public string Ozet { get; set; } = string.Empty;
        public string TamMetin { get; set; } = string.Empty;
    }
}
