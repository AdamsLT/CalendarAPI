namespace Calendarize.Infrastructure.Mongo
{
    public class MongoConfig
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public MongoCollections Collections { get; set; }
    }
}
