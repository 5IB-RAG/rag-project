﻿using Dapper;
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
        #region variabili
        public string sshHost = "158.180.235.79";
        public string database = "embeddingdb";
        public string username = "embedding";
        public string password = "Embedding2024@";
        public string sshUsername = "ubuntu";
        public string sshKeyFilePath = "../../../ssh-key-2024-01-26.key";
        public int sshPort = 22;
        public string dbHost = "localhost";
        public int dbPort = 3306;
        public int localPort = 3307;
        public string azureApiKey = "5619cf938d0241c2a7bc08eb7a692a83";
        public string azureEndpoint = "https://Passoni-Embedding.openai.azure.com/openai/deployments/ada-embedding/embeddings?api-version=2023-03-15-preview";
        #endregion

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

        public async Task RichiestaEmbedding(string testText)
        {
            if (testText is null || testText == "")
            {
                return;
            }
            Console.WriteLine("richiesta partita");
            // Invia la richiesta e ottieni la risposta
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("api-key", azureApiKey);
                var inputContent = new { input = testText };
                var json = JsonSerializer.Serialize(inputContent);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.WriteLine(content.Headers);
                var response = await client.PostAsync(azureEndpoint, content);
                Console.WriteLine(response);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    // Handle the error
                }
            }
        }

        static async Task Main(string[] args)
        {
            var connString = "Host=localhost;Username=postgres;Password=Embedding2024@;Database=embeddingdb";

            // Crea una connessione al database
            using var conn = new NpgsqlConnection(connString);
            Console.WriteLine("Tentativo di Connessione...");
            await conn.OpenAsync();
            Console.WriteLine("Connesso al Database");
            //// Esegui un'altra query (esempio di creazione tabella)
            //using var cmd2 = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS test_table (id SERIAL PRIMARY KEY, name VARCHAR(50))", conn);
            //cmd2.ExecuteNonQuery();

            //// Esegui un'istruzione di inserimento
            //using var cmd3 = new NpgsqlCommand("INSERT INTO test_table (name) VALUES (@name)", conn);
            //cmd3.Parameters.AddWithValue("name", "John Doe");
            //cmd3.ExecuteNonQuery();

            // Esegui una query di selezione
            using var cmd4 = new NpgsqlCommand("SELECT * FROM chiaro", conn);
            using var reader2 = await cmd4.ExecuteReaderAsync();
            while (reader2.Read())
            {
                Console.WriteLine($"ID: {reader2.GetInt32(0)}, Name: {reader2.GetString(1)}");
            }
            await conn.CloseAsync();
        }
    }
}