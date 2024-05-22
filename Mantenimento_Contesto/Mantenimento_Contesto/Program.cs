using Dapper;
using MySqlConnector;
using Org.BouncyCastle.Asn1.Mozilla;
using Renci.SshNet;
using System.Data;
using System.Text.Json;
using System.Text;
using Npgsql;
using Mantenimento_Contesto.Model;

namespace Mantenimento_Contesto
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = "Host=89.168.20.39;Username=postgres;Password=postgres;Database=postgres";
            string richiestaAzure = "ciao, parlami del videogioco Lupus";

            await Database.OpenConnection(connectionString);

            Response response = await Embedding.RichiestaEmbedding(richiestaAzure);

            int id_richiesta = await Database.CreateTesto(richiestaAzure);
            await Database.CreateEmbedding(Database.CastRequestToEmbeddingData(response), id_richiesta);
            await Database.GetAllTesto();
            await Database.GetAllEmbedding();
            await Database.CloseConnection();
        }
    }
}