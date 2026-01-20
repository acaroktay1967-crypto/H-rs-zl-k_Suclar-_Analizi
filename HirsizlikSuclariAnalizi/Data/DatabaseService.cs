using Microsoft.Data.Sqlite;
using HirsizlikSuclariAnalizi.Models;
using System.IO;

namespace HirsizlikSuclariAnalizi.Data
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hirsizlik.db");
            _connectionString = $"Data Source={dbPath}";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var createTablesCommand = connection.CreateCommand();
            createTablesCommand.CommandText = @"
                CREATE TABLE IF NOT EXISTS CezaHesaplama (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    HirsizlikTuru INTEGER NOT NULL,
                    MalDegeri REAL NOT NULL,
                    GeceVakti INTEGER NOT NULL,
                    EtkinPismanlik INTEGER NOT NULL,
                    MinCeza REAL NOT NULL,
                    MaxCeza REAL NOT NULL,
                    HesaplamaTarihi TEXT NOT NULL,
                    Notlar TEXT
                );

                CREATE TABLE IF NOT EXISTS YargitayKarari (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    KararNumarasi TEXT NOT NULL,
                    SucTuru INTEGER NOT NULL,
                    KararTarihi TEXT NOT NULL,
                    Ozet TEXT NOT NULL,
                    TamMetin TEXT NOT NULL
                );
            ";
            createTablesCommand.ExecuteNonQuery();

            // Insert sample court decisions if table is empty
            var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = "SELECT COUNT(*) FROM YargitayKarari";
            var count = (long)checkCommand.ExecuteScalar()!;
            
            if (count == 0)
            {
                InsertSampleData(connection);
            }
        }

        private void InsertSampleData(SqliteConnection connection)
        {
            var insertCommand = connection.CreateCommand();
            insertCommand.CommandText = @"
                INSERT INTO YargitayKarari (KararNumarasi, SucTuru, KararTarihi, Ozet, TamMetin) VALUES
                ('2019/1234', 0, '2019-03-15', 'Basit hırsızlık suçunda mal değerinin önemi', 'TCK 141. madde uyarınca basit hırsızlık suçunda malın değeri ceza miktarını etkiler. Düşük değerdeki mallarda ceza alt sınırdan uygulanmalıdır.'),
                ('2020/5678', 1, '2020-06-22', 'Gece vakti işlenen hırsızlık nitelikli hırsızlıktır', 'TCK 142. madde uyarınca gece vakti işlenen hırsızlık nitelikli hırsızlık sayılır ve ceza yarı oranında artırılır.'),
                ('2021/9012', 1, '2021-09-10', 'Etkin pişmanlık hükümlerinin uygulanması', 'Failin malı geri vermesi veya zararı gidermesi halinde etkin pişmanlık hükümleri uygulanarak ceza üçte birine kadar indirilebilir.'),
                ('2022/3456', 0, '2022-01-05', 'Basit hırsızlıkta zincirleme suç', 'Aynı kişiye karşı aynı suç işleme kararının devamı olarak farklı zamanlarda işlenen basit hırsızlık suçlarında zincirleme suç hükümleri uygulanır.'),
                ('2022/7890', 1, '2022-11-30', 'Konut dokunulmazlığını ihlalle hırsızlık', 'TCK 142/2-b maddesi uyarınca konut dokunulmazlığını ihlal suretiyle işlenen hırsızlık nitelikli hırsızlıktır.');
            ";
            insertCommand.ExecuteNonQuery();
        }

        public void SaveCezaHesaplama(CezaHesaplama ceza)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO CezaHesaplama (HirsizlikTuru, MalDegeri, GeceVakti, EtkinPismanlik, MinCeza, MaxCeza, HesaplamaTarihi, Notlar)
                VALUES (@HirsizlikTuru, @MalDegeri, @GeceVakti, @EtkinPismanlik, @MinCeza, @MaxCeza, @HesaplamaTarihi, @Notlar)
            ";
            command.Parameters.AddWithValue("@HirsizlikTuru", (int)ceza.HirsizlikTuru);
            command.Parameters.AddWithValue("@MalDegeri", ceza.MalDegeri);
            command.Parameters.AddWithValue("@GeceVakti", ceza.GeceVakti ? 1 : 0);
            command.Parameters.AddWithValue("@EtkinPismanlik", ceza.EtkinPismanlik ? 1 : 0);
            command.Parameters.AddWithValue("@MinCeza", ceza.MinCeza);
            command.Parameters.AddWithValue("@MaxCeza", ceza.MaxCeza);
            command.Parameters.AddWithValue("@HesaplamaTarihi", ceza.HesaplamaTarihi.ToString("o"));
            command.Parameters.AddWithValue("@Notlar", ceza.Notlar ?? string.Empty);
            command.ExecuteNonQuery();
        }

        public List<YargitayKarari> GetYargitayKararlari(string? kararNumarasi = null, HirsizlikTuru? sucTuru = null)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            var conditions = new List<string>();

            if (!string.IsNullOrWhiteSpace(kararNumarasi))
            {
                conditions.Add("KararNumarasi LIKE @KararNumarasi");
            }

            if (sucTuru.HasValue)
            {
                conditions.Add("SucTuru = @SucTuru");
            }

            command.CommandText = "SELECT * FROM YargitayKarari";
            if (conditions.Count > 0)
            {
                command.CommandText += " WHERE " + string.Join(" AND ", conditions);
            }

            if (!string.IsNullOrWhiteSpace(kararNumarasi))
            {
                command.Parameters.AddWithValue("@KararNumarasi", $"%{kararNumarasi}%");
            }

            if (sucTuru.HasValue)
            {
                command.Parameters.AddWithValue("@SucTuru", (int)sucTuru.Value);
            }

            var kararlar = new List<YargitayKarari>();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                kararlar.Add(new YargitayKarari
                {
                    Id = reader.GetInt32(0),
                    KararNumarasi = reader.GetString(1),
                    SucTuru = (HirsizlikTuru)reader.GetInt32(2),
                    KararTarihi = DateTime.Parse(reader.GetString(3)),
                    Ozet = reader.GetString(4),
                    TamMetin = reader.GetString(5)
                });
            }

            return kararlar;
        }

        public List<CezaHesaplama> GetCezaHesaplamalari()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM CezaHesaplama ORDER BY HesaplamaTarihi DESC";

            var hesaplamalar = new List<CezaHesaplama>();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                hesaplamalar.Add(new CezaHesaplama
                {
                    Id = reader.GetInt32(0),
                    HirsizlikTuru = (HirsizlikTuru)reader.GetInt32(1),
                    MalDegeri = (decimal)reader.GetDouble(2),
                    GeceVakti = reader.GetInt32(3) == 1,
                    EtkinPismanlik = reader.GetInt32(4) == 1,
                    MinCeza = reader.GetDouble(5),
                    MaxCeza = reader.GetDouble(6),
                    HesaplamaTarihi = DateTime.Parse(reader.GetString(7)),
                    Notlar = reader.IsDBNull(8) ? null : reader.GetString(8)
                });
            }

            return hesaplamalar;
        }
    }
}
