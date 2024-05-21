using Google.Protobuf.WellKnownTypes;
using Mantenimento_Contesto.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantenimento_Contesto
{
    public class Database
    {
        public string connString;
        public NpgsqlConnection? conn;

        public Database()
        {

        }

        public async Task Connection(string connectionString)
        {
            if (connectionString == null || connectionString == "")
            {
                return;
            }

            // Crea una connessione al database
            conn = new NpgsqlConnection(connectionString);
            Console.WriteLine("Tentativo di Connessione...");
            await conn.OpenAsync();
            Console.WriteLine("Connesso al Database");
        }

        public async Task CloseConnection()
        {
            Console.WriteLine("Chiusura della connessione...");
            await conn.CloseAsync();
            Console.WriteLine("Chiusura della connessione!");
        }

        public async Task GetAll()
        {
            using var cmd4 = new NpgsqlCommand("SELECT * FROM cleanrequest", conn);
            using var reader2 = await cmd4.ExecuteReaderAsync();
            while (reader2.Read())
            {
                Console.WriteLine($"ID: {reader2.GetInt32(0)}, Name: {reader2.GetString(1)}");
            }
        }

        public async Task Create(string request)
        {
            var query = "INSERT INTO cleanrequest (text) VALUES @request";
            await using var cmd = new NpgsqlCommand(query, conn);
        }
    }
}
