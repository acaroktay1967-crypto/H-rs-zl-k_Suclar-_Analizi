namespace HirsizlikSuclariAnalizi.Models
{
    /// <summary>
    /// Hesaplanan ceza sonucu
    /// </summary>
    public class CezaSonucu
    {
        /// <summary>
        /// Temel ceza alt sınırı (ay)
        /// </summary>
        public int TemelCezaAltSinir { get; set; }
        
        /// <summary>
        /// Temel ceza üst sınırı (ay)
        /// </summary>
        public int TemelCezaUstSinir { get; set; }
        
        /// <summary>
        /// Nitelikli hallere göre artırılmış ceza alt sınırı (ay)
        /// </summary>
        public int ArtirimisCezaAltSinir { get; set; }
        
        /// <summary>
        /// Nitelikli hallere göre artırılmış ceza üst sınırı (ay)
        /// </summary>
        public int ArtirimisCezaUstSinir { get; set; }
        
        /// <summary>
        /// Tayin edilecek nihai ceza alt sınırı (ay)
        /// </summary>
        public int NihaiCezaAltSinir { get; set; }
        
        /// <summary>
        /// Tayin edilecek nihai ceza üst sınırı (ay)
        /// </summary>
        public int NihaiCezaUstSinir { get; set; }
        
        /// <summary>
        /// Uygulanan indirimler ve artırımlar açıklaması
        /// </summary>
        public List<string> Aciklamalar { get; set; } = new List<string>();
        
        /// <summary>
        /// Para cezası (TL) - Alternatif yaptırım
        /// </summary>
        public decimal? ParaCezasi { get; set; }
        
        /// <summary>
        /// Hesaplamada kullanılan yasal maddeler
        /// </summary>
        public List<string> YasalDayanaklar { get; set; } = new List<string>();
    }
}
