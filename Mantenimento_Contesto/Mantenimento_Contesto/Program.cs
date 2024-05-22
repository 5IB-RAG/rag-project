using Dapper;
using MySqlConnector;
using Org.BouncyCastle.Asn1.Mozilla;
using Renci.SshNet;
using System.Data;
using System.Text.Json;
using System.Text;
using Npgsql;

namespace Mantenimento_Contesto
{
    internal class Program
    {
        #region dbOracle
        //public async Task InterfacciaDb()
        //{
        //    var privateKeyFile = new PrivateKeyFile(sshKeyFilePath);
        //    var authenticationMethod = new PrivateKeyAuthenticationMethod(sshUsername, privateKeyFile);
        //    var connectionInfo = new ConnectionInfo(sshHost, sshPort, sshUsername, authenticationMethod);

        //    using (var client = new SshClient(connectionInfo))
        //    {
        //        client.Connect();
        //        var portForwarded = new ForwardedPortLocal("127.0.0.1", (uint)localPort, dbHost, (uint)dbPort);
        //        client.AddForwardedPort(portForwarded);
        //        portForwarded.Start();

        //        // Ora puoi connetterti al database tramite localhost:3307
        //        string connectionString = $"Server=127.0.0.1;Port={localPort};Database={database};User Id={username};Password={password};";
        //        using (var connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
        //        {
        //            Console.WriteLine("tentativo di connessione...");
        //            connection.Open();
        //            Console.WriteLine("tentativo riuscito!");
        //            string query = "INSERT testoinchiaro FROM chiaro"; // Sostituisci con una tua query
        //            var count = await connection.QueryAsync<string>(query);

        //            Console.WriteLine("il numero di righe sono: " + count.ToList()[0]);
        //            // Esegui le operazioni sul database
        //        }
        //        portForwarded.Stop();
        //        client.Disconnect();
        //    }
        //}
        #endregion

        static async Task Main(string[] args)
        {
            #region commenti
            //// Esegui un'altra query (esempio di creazione tabella)
            //using var cmd2 = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS test_table (id SERIAL PRIMARY KEY, name VARCHAR(50))", conn);
            //cmd2.ExecuteNonQuery();

            //"Host=localhost;Username=postgres;Password=postgres;Database=postgres"

            //// Esegui un'istruzione di inserimento
            //using var cmd3 = new NpgsqlCommand("INSERT INTO test_table (name) VALUES (@name)", conn);
            //cmd3.Parameters.AddWithValue("name", "John Doe");
            //cmd3.ExecuteNonQuery();

            // Esegui una query di selezione
            #endregion

            Database d = new Database();
            Embedding e = new Embedding();
            await d.Connection("Host=89.168.20.39;Username=postgres;Password=postgres;Database=postgres");
            await d.GetAll();
            //await e.RichiestaEmbedding("prova 1, prova 2, prova3");
            //await d.Create("prova 1, prova 2, prova3");
            await d.Delete(6);
            await d.Delete(5);
            await d.GetAll();
        }
    }
}