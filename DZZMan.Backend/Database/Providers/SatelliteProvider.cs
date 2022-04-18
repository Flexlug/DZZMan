using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using DZZMan.Models;

namespace DZZMan.Backend.Database.Providers
{
    public class SatelliteProvider
    {
        private readonly IDocumentStore _store;
        private readonly ILogger<SatelliteProvider> _logger;

        public SatelliteProvider(ILogger<SatelliteProvider> logger,
                                 DocumentStoreProvider store)
        {
            _store = store.Store;
            _logger = logger;
        }

        public async Task<List<Satellite>> GetSatellitesAsync()
        {
            using (IAsyncDocumentSession session = _store.OpenAsyncSession(
                new Raven.Client.Documents.Session.SessionOptions() { NoTracking = true }))
            {
                var alreadySubmitedMap = await session
                    .Query<Satellite>()
                    .Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5)))
                    .ToListAsync();

                return alreadySubmitedMap;
            }
        }

        public async Task<Satellite> GetSatelliteAsync(string name)
        {
            using (IAsyncDocumentSession session = _store.OpenAsyncSession(
                new Raven.Client.Documents.Session.SessionOptions() { NoTracking = true }))
            {
                return await session
                    .Query<Satellite>()
                    .FirstOrDefaultAsync(x => x.Name == name);
            }
        }

        public async Task AddSatelliteAsync(Satellite satellite)
        {
            using (IAsyncDocumentSession session = _store.OpenAsyncSession())
            {
                await session.StoreAsync(satellite);
                await session.SaveChangesAsync();
            }
        }

        public async Task UpdateSatelliteAsync(string name, Satellite satellite)
        {
            using (IAsyncDocumentSession session = _store.OpenAsyncSession())
            {
                var oldSatelite = await session
                    .Query<Satellite>()
                    .FirstOrDefaultAsync(x => x.Name == name) 
                    ?? throw new NullReferenceException("Couldn't find such satellite");

                session.Delete(oldSatelite);

                await session.StoreAsync(satellite);
                await session.SaveChangesAsync();
            }
        }

        public async Task DeleteSatelliteAsync(string name)
        {
            using (IAsyncDocumentSession session = _store.OpenAsyncSession())
            {
                var oldSatelite = await session
                    .Query<Satellite>()
                    .FirstOrDefaultAsync(x => x.Name == name)
                    ?? throw new NullReferenceException("Couldn't find such satellite");

                session.Delete(oldSatelite);

                await session.SaveChangesAsync();
            }
        }
    }
}
