using Marten;
using Marten.Services;

namespace Tracking.Api.Data
{
    public class MartenDomainEventDispatcher : IDocumentSessionListener
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MartenDomainEventDispatcher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public Task AfterCommitAsync(IDocumentSession session, IChangeSet commit, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task BeforeSaveChangesAsync(IDocumentSession session, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public void DocumentAddedForStorage(object id, object document)
        {
            throw new NotImplementedException();
        }

        public void DocumentLoaded(object id, object document)
        {
            throw new NotImplementedException();
        }
    }
}
