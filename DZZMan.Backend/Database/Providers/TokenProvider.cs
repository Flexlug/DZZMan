using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace DZZMan.Backend.Database.Providers
{
    public class TokenProvider
    {
        private readonly IDocumentStore _store;
        private readonly ILogger<TokenProvider> _logger;

        public TokenProvider(ILogger<TokenProvider> logger,
                             DocumentStoreProvider store)
        {
            _store = store.Store;
            _logger = logger;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            using (IAsyncDocumentSession session = _store.OpenAsyncSession())
            {
                var tokenCollection = await session
                    .Query<TokenCollection>()
                    .FirstOrDefaultAsync();

                if (tokenCollection is null)
                {
                    tokenCollection = new() { Tokens = new List<string>() { $"{GetHashCode()}-{DateTime.Now.Ticks}" } };
                    await session.StoreAsync(tokenCollection);
                    await session.SaveChangesAsync();

                    return false;
                }

                if (tokenCollection.Tokens.Any(x => x.Equals(token)))
                    return true;

                return false;
            }
        }

        public class TokenCollection
        {
            public List<string> Tokens { get; set; } = new();
        }
    }
}
