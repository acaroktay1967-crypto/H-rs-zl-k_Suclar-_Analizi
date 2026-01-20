namespace HirsizlikSuclariAnalizi.Models
{
    /// <summary>
    /// Ceza hesaplama için gerekli parametreler
    /// </summary>
    public class CezaHesaplamaParametreleri
    {
        /// <summary>
        /// Hırsızlık türü
        /// </summary>
        public HirsizlikTuru HirsizlikTuru { get; set; }
        
        /// <summary>
        /// Nitelikli hırsızlık sebepleri
        /// </summary>
        public List<NitelikliBolum> NitelikliSebepler { get; set; } = new List<NitelikliBolum>();
        
        /// <summary>
        /// Çalınan malın değeri (TL)
        /// </summary>
        public decimal MalDegeri { get; set; }
        
        /// <summary>
        /// Etkin pişmanlık var mı?
        /// </summary>
        public bool EtkinPismanlik { get; set; }
        
        /// <summary>
        /// Uzlaştırma var mı?
        /// </summary>
        public bool Uzlastirma { get; set; }
        
        /// <summary>
        /// Fail ile mağdur arasında akrabalık ilişkisi var mı?
        /// </summary>
        public bool AkrabaMagdur { get; set; }
        
        /// <summary>
        /// Teşebbüs aşamasında mı kaldı?
        /// </summary>
        public bool Tesebbüs { get; set; }
        
        /// <summary>
        /// Sanığın cezası ertelenebilir durumda mı?
        /// </summary>
        public bool HukmunAciklanmasininGeribirakılmasi { get; set; }
    }
}
