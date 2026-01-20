namespace HirsizlikSuclariAnalizi.Models
{
    /// <summary>
    /// Yargıtay kararı modeli
    /// </summary>
    public class YargitayKarari
    {
        /// <summary>
        /// Karar ID
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Yargıtay Daire No
        /// </summary>
        public string? DaireNo { get; set; }
        
        /// <summary>
        /// Esas No
        /// </summary>
        public string? EsasNo { get; set; }
        
        /// <summary>
        /// Karar No
        /// </summary>
        public string? KararNo { get; set; }
        
        /// <summary>
        /// Karar Tarihi
        /// </summary>
        public DateTime KararTarihi { get; set; }
        
        /// <summary>
        /// Karar Özeti
        /// </summary>
        public string Ozet { get; set; } = string.Empty;
        
        /// <summary>
        /// Karar Metni
        /// </summary>
        public string KararMetni { get; set; } = string.Empty;
        
        /// <summary>
        /// İlgili TCK Maddesi
        /// </summary>
        public string? TCKMaddesi { get; set; }
        
        /// <summary>
        /// Anahtar Kelimeler
        /// </summary>
        public string? AnahtarKelimeler { get; set; }
    }
}
