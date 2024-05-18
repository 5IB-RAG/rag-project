using Dapper;
using MySqlConnector;
using Renci.SshNet;
using System.Data;

namespace Mantenimento_Contesto
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //indirizzo ip: 158
            //password db: Embedding2024@
            //utente db: embedding
            string sshHost = "158.180.235.79";
            string database = "embeddingdb";
            string username = "embedding";
            string password = "Embedding2024@";

            string sshUsername = "ubuntu";
            string sshKeyFilePath = "../../../ssh-key-2024-01-26.key";
            int sshPort = 22;

            string dbHost = "localhost";
            int dbPort = 3306;
            int localPort = 3307;

            var privateKeyFile = new PrivateKeyFile(sshKeyFilePath);
            var authenticationMethod = new PrivateKeyAuthenticationMethod(sshUsername, privateKeyFile);
            var connectionInfo = new ConnectionInfo(sshHost, sshPort, sshUsername, authenticationMethod);

            using (var client = new SshClient(connectionInfo))
            {
                client.Connect();
                var portForwarded = new ForwardedPortLocal("127.0.0.1", (uint)localPort, dbHost, (uint)dbPort);
                client.AddForwardedPort(portForwarded);
                portForwarded.Start();

                // Ora puoi connetterti al database tramite localhost:3307
                string connectionString = $"Server=127.0.0.1;Port={localPort};Database={database};User Id={username};Password={password};";
                using (var connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
                {
                    Console.WriteLine("tentativo di connessione...");
                    connection.Open();
                    Console.WriteLine("tentativo riuscito!");
                    string query = "SELECT testoinchiaro FROM chiaro"; // Sostituisci con una tua query
                    var count = await connection.QueryAsync<string>(query);

                    Console.WriteLine("il numero di righe sono: " + count.ToList()[0]);
                    // Esegui le operazioni sul database
                }

                portForwarded.Stop();
                client.Disconnect();
            }
        }
    }
}
