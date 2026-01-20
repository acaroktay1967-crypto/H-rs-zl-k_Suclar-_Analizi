using Microsoft.Data.Sqlite;
using HirsizlikSuclariAnalizi.Models;

namespace HirsizlikSuclariAnalizi.Data
{
    /// <summary>
    /// Yargıtay kararları için veritabanı servisi
    /// </summary>
    public class YargitayVeriTabaniServisi
    {
        private readonly string _connectionString;
        
        public YargitayVeriTabaniServisi(string dbPath = "yargitay.db")
        {
            _connectionString = $"Data Source={dbPath}";
            VeriTabaniOlustur();
        }
        
        /// <summary>
        /// Veritabanı tablolarını oluşturur
        /// </summary>
        private void VeriTabaniOlustur()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            
            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS YargitayKararlari (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    DaireNo TEXT,
                    EsasNo TEXT,
                    KararNo TEXT,
                    KararTarihi TEXT NOT NULL,
                    Ozet TEXT NOT NULL,
                    KararMetni TEXT NOT NULL,
                    TCKMaddesi TEXT,
                    AnahtarKelimeler TEXT
                );
                
                CREATE INDEX IF NOT EXISTS idx_tck_maddesi ON YargitayKararlari(TCKMaddesi);
                CREATE INDEX IF NOT EXISTS idx_karar_tarihi ON YargitayKararlari(KararTarihi);
            ";
            command.ExecuteNonQuery();
            
            // Örnek veriler ekle
            OrnekVerileriEkle(connection);
        }
        
        /// <summary>
        /// Örnek Yargıtay kararlarını ekler
        /// </summary>
        private void OrnekVerileriEkle(SqliteConnection connection)
        {
            var countCommand = connection.CreateCommand();
            countCommand.CommandText = "SELECT COUNT(*) FROM YargitayKararlari";
            var count = Convert.ToInt32(countCommand.ExecuteScalar());
            
            if (count > 0) return; // Zaten veri var
            
            var ornekKararlar = new[]
            {
                new {
                    DaireNo = "4. Ceza Dairesi",
                    EsasNo = "2020/12345",
                    KararNo = "2020/6789",
                    KararTarihi = "2020-05-15",
                    Ozet = "Gece vakti işlenen hırsızlık suçunda nitelikli hal uygulanması",
                    KararMetni = "Sanığın, gece vakti mağdurun evine girerek hırsızlık yapması nedeniyle TCK 142/1-a maddesinin uygulanması gerektiği, mahkemenin basit hırsızlık olarak kabul etmesinin hatalı olduğu...",
                    TCKMaddesi = "142/1-a",
                    AnahtarKelimeler = "gece vakti, nitelikli hırsızlık, mesken"
                },
                new {
                    DaireNo = "4. Ceza Dairesi",
                    EsasNo = "2019/8765",
                    KararNo = "2020/1234",
                    KararTarihi = "2020-03-20",
                    Ozet = "İki veya daha fazla kişi ile birlikte işlenen hırsızlık",
                    KararMetni = "Sanıkların, birlikte hareket ederek mağdurun işyerine girmesi ve eşyaları çalması halinde TCK 142/1-b maddesinin uygulanması gerektiği...",
                    TCKMaddesi = "142/1-b",
                    AnahtarKelimeler = "birlikte suç, iştirak, nitelikli hırsızlık"
                },
                new {
                    DaireNo = "4. Ceza Dairesi",
                    EsasNo = "2021/3456",
                    KararNo = "2021/7890",
                    KararTarihi = "2021-09-10",
                    Ozet = "Etkin pişmanlık hükümlerinin uygulanması",
                    KararMetni = "Sanığın, suç konusu malı geri vermesi ve zararı tazmin etmesi halinde TCK 168. madde gereğince etkin pişmanlık hükümlerinden yararlanabileceği...",
                    TCKMaddesi = "168",
                    AnahtarKelimeler = "etkin pişmanlık, zarar tazmini"
                },
                new {
                    DaireNo = "4. Ceza Dairesi",
                    EsasNo = "2019/5678",
                    KararNo = "2020/2345",
                    KararTarihi = "2020-01-25",
                    Ozet = "Akrabalar arasında işlenen hırsızlık suçu",
                    KararMetni = "Üstsoy veya altsoydan birine karşı işlenen hırsızlık suçunun şikayete tabi olduğu, mağdurun şikayetçi olmaması halinde davanın düşürüleceği (TCK 167)...",
                    TCKMaddesi = "167",
                    AnahtarKelimeler = "akraba, şikayete tabi, üstsoy, altsoy"
                },
                new {
                    DaireNo = "4. Ceza Dairesi",
                    EsasNo = "2020/9876",
                    KararNo = "2021/3456",
                    KararTarihi = "2021-04-12",
                    Ozet = "Teşebbüs aşamasında kalan hırsızlık suçu",
                    KararMetni = "Sanığın hırsızlık amacıyla mağdurun evine girmesi ancak herhangi bir eşya alamaması halinde teşebbüs hükümlerinin uygulanacağı (TCK 35)...",
                    TCKMaddesi = "35, 141",
                    AnahtarKelimeler = "teşebbüs, icra hareketleri"
                }
            };
            
            foreach (var karar in ornekKararlar)
            {
                var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = @"
                    INSERT INTO YargitayKararlari 
                    (DaireNo, EsasNo, KararNo, KararTarihi, Ozet, KararMetni, TCKMaddesi, AnahtarKelimeler)
                    VALUES (@DaireNo, @EsasNo, @KararNo, @KararTarihi, @Ozet, @KararMetni, @TCKMaddesi, @AnahtarKelimeler)
                ";
                insertCommand.Parameters.AddWithValue("@DaireNo", karar.DaireNo);
                insertCommand.Parameters.AddWithValue("@EsasNo", karar.EsasNo);
                insertCommand.Parameters.AddWithValue("@KararNo", karar.KararNo);
                insertCommand.Parameters.AddWithValue("@KararTarihi", karar.KararTarihi);
                insertCommand.Parameters.AddWithValue("@Ozet", karar.Ozet);
                insertCommand.Parameters.AddWithValue("@KararMetni", karar.KararMetni);
                insertCommand.Parameters.AddWithValue("@TCKMaddesi", karar.TCKMaddesi);
                insertCommand.Parameters.AddWithValue("@AnahtarKelimeler", karar.AnahtarKelimeler);
                insertCommand.ExecuteNonQuery();
            }
        }
        
        /// <summary>
        /// Tüm Yargıtay kararlarını getirir
        /// </summary>
        public List<YargitayKarari> TumKararlariGetir()
        {
            var kararlar = new List<YargitayKarari>();
            
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM YargitayKararlari ORDER BY KararTarihi DESC";
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                kararlar.Add(OkuyucudanKararOlustur(reader));
            }
            
            return kararlar;
        }
        
        /// <summary>
        /// Anahtar kelimeye göre arama yapar
        /// </summary>
        public List<YargitayKarari> KararAra(string aramaMetni)
        {
            var kararlar = new List<YargitayKarari>();
            
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            
            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT * FROM YargitayKararlari 
                WHERE Ozet LIKE @Arama 
                   OR KararMetni LIKE @Arama 
                   OR AnahtarKelimeler LIKE @Arama
                   OR TCKMaddesi LIKE @Arama
                ORDER BY KararTarihi DESC
            ";
            command.Parameters.AddWithValue("@Arama", $"%{aramaMetni}%");
            
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                kararlar.Add(OkuyucudanKararOlustur(reader));
            }
            
            return kararlar;
        }
        
        /// <summary>
        /// SqlDataReader'dan YargitayKarari nesnesi oluşturur
        /// </summary>
        private YargitayKarari OkuyucudanKararOlustur(SqliteDataReader reader)
        {
            return new YargitayKarari
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                DaireNo = reader.IsDBNull(reader.GetOrdinal("DaireNo")) ? null : reader.GetString(reader.GetOrdinal("DaireNo")),
                EsasNo = reader.IsDBNull(reader.GetOrdinal("EsasNo")) ? null : reader.GetString(reader.GetOrdinal("EsasNo")),
                KararNo = reader.IsDBNull(reader.GetOrdinal("KararNo")) ? null : reader.GetString(reader.GetOrdinal("KararNo")),
                KararTarihi = DateTime.Parse(reader.GetString(reader.GetOrdinal("KararTarihi"))),
                Ozet = reader.GetString(reader.GetOrdinal("Ozet")),
                KararMetni = reader.GetString(reader.GetOrdinal("KararMetni")),
                TCKMaddesi = reader.IsDBNull(reader.GetOrdinal("TCKMaddesi")) ? null : reader.GetString(reader.GetOrdinal("TCKMaddesi")),
                AnahtarKelimeler = reader.IsDBNull(reader.GetOrdinal("AnahtarKelimeler")) ? null : reader.GetString(reader.GetOrdinal("AnahtarKelimeler"))
            };
        }
    }
}
