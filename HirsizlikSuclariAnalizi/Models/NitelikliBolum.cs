namespace HirsizlikSuclariAnalizi.Models
{
    /// <summary>
    /// TCK 142. maddesine göre nitelikli hırsızlık sebepleri
    /// </summary>
    public enum NitelikliBolum
    {
        /// <summary>
        /// Herhangi bir nitelikli sebep yok
        /// </summary>
        Yok,
        
        /// <summary>
        /// Gece vakti işlenmiş olması (TCK 142/1-a)
        /// </summary>
        GeceVakti,
        
        /// <summary>
        /// İki veya daha fazla kişi tarafından birlikte işlenmiş olması (TCK 142/1-b)
        /// </summary>
        BirlikteSuc,
        
        /// <summary>
        /// Beden veya ruh bakımından kendisini savunamayacak durumda bulunan kişiye karşı işlenmiş olması (TCK 142/1-c)
        /// </summary>
        SavunmasizMagdur,
        
        /// <summary>
        /// Bir kamu binasında veya eklentilerinde işlenmiş olması (TCK 142/1-d)
        /// </summary>
        KamuBinasi,
        
        /// <summary>
        /// Yangın, su baskını, deprem, salgın hastalık gibi felâket durumlarından yararlanmak suretiyle işlenmiş olması (TCK 142/1-e)
        /// </summary>
        FelaketDurumu,
        
        /// <summary>
        /// Dini hassasiyetle bağlantılı yerde işlenmiş olması (TCK 142/2-a)
        /// </summary>
        DiniYer,
        
        /// <summary>
        /// Eğitim, sağlık veya sosyal hizmet verilen yerlerde işlenmiş olması (TCK 142/2-b)
        /// </summary>
        HizmetYeri,
        
        /// <summary>
        /// Taşıt terminali, mesken, işyeri, iş makinesinde veya eklentilerinde işlenmiş olması (TCK 142/2-c)
        /// </summary>
        MeskenIsyeri
    }
}
