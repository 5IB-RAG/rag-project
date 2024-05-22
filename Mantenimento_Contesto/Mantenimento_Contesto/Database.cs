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
        public static NpgsqlConnection? conn;

        public Database()
        {

        }

        public static async Task<int> OpenConnection(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Console.WriteLine("Stringa di connessione non valida.");
                return -1;
            }

            try
            {
                // Crea una connessione al database
                conn = new NpgsqlConnection(connectionString);
                Console.WriteLine("Tentativo di Connessione...");
                await conn.OpenAsync();
                Console.WriteLine("Connesso al Database");
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante la connessione: {ex.Message}");
                return -1;
            }
        }

        public static async Task CloseConnection()
        {
            if (conn != null)
            {
                try
                {
                    Console.WriteLine("Chiusura della connessione...");
                    await conn.CloseAsync();
                    Console.WriteLine("Connessione chiusa!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Errore durante la chiusura della connessione: {ex.Message}");
                }
            }
        }

        public static async Task GetAllTesto()
        {
            if (conn == null)
            {
                Console.WriteLine("Connessione non inizializzata.");
                return;
            }

            try
            {
                using var cmd4 = new NpgsqlCommand("SELECT * FROM cleanrequest", conn);
                using var reader2 = await cmd4.ExecuteReaderAsync();
                while (reader2.Read())
                {
                    Console.WriteLine($"ID: {reader2.GetInt32(0)}, Text: {reader2.GetString(1)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il recupero dei dati: {ex.Message}");
            }
        }
        public static async Task GetTesto(int id)
        {
            if (conn is null)
            {
                Console.WriteLine("Connessione non inizializzata.");
                return;
            }
            try
            {
                using var cmd = new NpgsqlCommand("SELECT * FROM cleanrequest WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                using var reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader.GetInt32(0)}, Text: {reader.GetString(1)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il recupero dei dati: {ex.Message}");
            }
        }



        public static async Task CreateTesto(string request)
        {
            if (conn == null || string.IsNullOrWhiteSpace(request))
            {
                Console.WriteLine("Connessione non inizializzata o richiesta non valida.");
                return;
            }

            try
            {
                var query = "INSERT INTO cleanrequest (text) VALUES (@request)";
                await using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@request", request);
                await cmd.ExecuteNonQueryAsync();
                Console.WriteLine("Inserimento completato.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'inserimento dei dati: {ex.Message}");
            }
        }

        public static async Task CreateEmbedding(string request)
        {
            if (conn == null || string.IsNullOrWhiteSpace(request))
            {
                Console.WriteLine("Connessione non inizializzata o richiesta non valida.");
                return;
            }

            try
            {
                var query = "INSERT INTO  (text) VALUES (@request)";
                await using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@request", request);
                await cmd.ExecuteNonQueryAsync();
                Console.WriteLine("Inserimento completato.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'inserimento dei dati: {ex.Message}");
            }
        }

        public static async Task Delete(int id)
        {
            if (conn == null)
            {
                Console.WriteLine("Connessione non inizializzata.");
                return;
            }

            try
            {
                var query = "DELETE FROM cleanrequest WHERE id = @id";
                await using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Record con ID {id} eliminato.");
                }
                else
                {
                    Console.WriteLine($"Nessun record trovato con ID {id}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'eliminazione dei dati: {ex.Message}");
            }
        }
    }
}