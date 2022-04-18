using Raven.Client.Documents;

namespace DZZMan.Backend.Database
{
    public class DocumentStoreProvider
    {
        private static ILogger<DocumentStoreProvider> _logger;

        private static string IP;
        private static string Name;
        private static string Port;

        public DocumentStoreProvider(Settings settings, ILogger<DocumentStoreProvider> logger)
        {
            _logger = logger;

            Name = settings.DbName;
            IP = settings.DbIp;
            Port = settings.DbPort;
        }


        private IDocumentStore _store = null;
        public IDocumentStore Store
        {

            get
            {
                if (_store == null)
                    _store = CreateStore();

                return _store;
            }
            private set
            {
                _store = value;
            }
        }

        private IDocumentStore CreateStore()
        {
            IDocumentStore store = new DocumentStore()
            {
                // Define the cluster node URLs (required)
                Urls = new[]
                {
                    $"http://{IP}:{Port}"
                },

                // Define a default database (optional)
                Database = Name,

                // Initialize the Document Store
            }.Initialize();

            return store;
        }
    }
}
