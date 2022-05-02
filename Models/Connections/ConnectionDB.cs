using MongoDB.Driver;

namespace SimpleTax
{
    public class ConnectionDB
    {
        public ConnectionDB()
        {
            client = new MongoClient(connectionString);
        }

        public MongoClient client { get; set; }

        //private string connectionString = "mongodb+srv://admin:admin@cluster0-wdztm.gcp.mongodb.net/admin";
        //private string connectionString = "mongodb://admin:<admin>@cluster0-shard-00-00-wdztm.gcp.mongodb.net:27017,cluster0-shard-00-01-wdztm.gcp.mongodb.net:27017,cluster0-shard-00-02-wdztm.gcp.mongodb.net:27017/?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin&retryWrites=true&w=majority";
        private string connectionString = "mongodb://localhost:27017/";
    }
}