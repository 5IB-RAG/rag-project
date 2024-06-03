using Google.Protobuf.WellKnownTypes;
using Mantenimento_Contesto.Model;
using Npgsql;
using Org.BouncyCastle.Crypto.Engines;
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
        #region state db
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
        #endregion

        #region cleantext
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

        public static async Task GetTestoById(int id)
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

        public static async Task<int> CreateTesto(string request)
        {
            if (conn == null || string.IsNullOrWhiteSpace(request))
            {
                Console.WriteLine("Connessione non inizializzata o richiesta non valida.");
                return -1;
            }

            try
            {
                var query = "INSERT INTO cleanrequest (Testo) VALUES (@request) RETURNING Id";
                await using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@request", request);
                // Eseguire il comando e ottenere l'ID restituito
                var id = await cmd.ExecuteScalarAsync();
                if (id != null)
                {
                    Console.WriteLine("Inserimento completato. ID assegnato: " + id.ToString());
                    return Convert.ToInt32(id);
                }
                else
                {
                    Console.WriteLine("Nessun ID restituito dopo l'inserimento.");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'inserimento dei dati: {ex.Message}");
                return -1;
            }
        }

        public static async Task DeleteTesto(int id)
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
        #endregion

        #region Embedding
        public static async Task GetAllEmbedding()
        {
            if (conn == null)
            {
                Console.WriteLine("Connessione non inizializzata.");
                return;
            }

            try
            {
                using var cmd = new NpgsqlCommand("SELECT * FROM embedding_data", conn);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int id = reader.GetInt32(reader.GetOrdinal("Id"));
                    double[] embeddingArray = (double[])reader["Embedding"];
                    int cleanRequestId = reader.GetInt32(reader.GetOrdinal("CleanRequestId"));

                    // Ora puoi fare ciò che vuoi con i dati recuperati
                    Console.WriteLine($"ID: {id}, Embedding: [{string.Join(", ", embeddingArray)}], CleanRequestId: {cleanRequestId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il recupero dei dati: {ex.Message}");
            }
        }

        public static async Task GetEmbeddingById(int idRequest)
        {
            if (conn is null)
            {
                Console.WriteLine("Connessione non inizializzata.");
                return;
            }

            try
            {
                using var cmd = new NpgsqlCommand("SELECT * FROM embedding_data WHERE Id = @idRequest", conn);
                cmd.Parameters.AddWithValue("@idRequest", idRequest);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int id = reader.GetInt32(reader.GetOrdinal("Id"));
                    double[] embeddingArray = (double[])reader["Embedding"];
                    int cleanRequestId = reader.GetInt32(reader.GetOrdinal("CleanRequestId"));

                    Console.WriteLine($"ID: {id}, Embedding: [{string.Join(", ", embeddingArray)}], CleanRequestId: {cleanRequestId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il recupero dei dati: {ex.Message}");
            }
        }

        public static async Task GetEmbeddingByForeignKey(int idRequest)
        {
            if (conn is null)
            {
                Console.WriteLine("Connessione non inizializzata.");
                return;
            }

            try
            {
                using var cmd = new NpgsqlCommand("SELECT * FROM embedding_data WHERE CleanRequestId = @idRequest", conn);
                cmd.Parameters.AddWithValue("@idRequest", idRequest);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int id = reader.GetInt32(reader.GetOrdinal("Id"));
                    double[] embeddingArray = (double[])reader["Embedding"];
                    int cleanRequestId = reader.GetInt32(reader.GetOrdinal("CleanRequestId"));

                    Console.WriteLine($"ID: {id}, Embedding: [{string.Join(", ", embeddingArray)}], CleanRequestId: {cleanRequestId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il recupero dei dati: {ex.Message}");
            }
        }

        public static async Task CreateEmbedding(EmbeddingData request, int fk_cleanrequest)
        {
            if (conn == null || request == null || fk_cleanrequest <= 0)
            {
                Console.WriteLine("Connessione non inizializzata o richiesta non valida.");
                return;
            }

            try
            {
                var embeddingArray = request.Embedding.ToArray();
                var query = "INSERT INTO embedding_data (Embedding, CleanRequestId) VALUES (@embedding, @cleanRequestId)";
                await using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@embedding", embeddingArray);
                cmd.Parameters.AddWithValue("@cleanRequestId", fk_cleanrequest);
                await cmd.ExecuteNonQueryAsync();
                Console.WriteLine("Inserimento completato.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'inserimento dei dati: {ex.Message}");
            }
        }

        public static async Task DeleteEmbedding(int id)
        {
            if (conn == null)
            {
                Console.WriteLine("Connessione non inizializzata.");
                return;
            }

            try
            {
                var query = "DELETE FROM embedding_data WHERE id = @id";
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
        #endregion

        public static EmbeddingData CastRequestToEmbeddingData(Response response)
        {
            if (response is null)
            {
                return null;
            }
            EmbeddingData embeddingData = new();
            embeddingData.Index = null;
            embeddingData.Object = response.Object;
            embeddingData.Embedding = response.Data[0].Embedding;
            return embeddingData;
        }
    }
}