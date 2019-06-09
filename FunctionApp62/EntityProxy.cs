using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;

namespace FunctionApp62
{
    public abstract class EntityProxy
    {
        protected EntityProxy(IEntityProxyContext context, EntityId entityId)
        {
            _context = context;
            _entityId = entityId;
        }

        private readonly IEntityProxyContext _context;
        private readonly EntityId _entityId;

        protected Task<TResult> InvokeAsync<TResult>(string operationName, object operationInput)
        {
            return _context.InvokeAsync<TResult>(_entityId, operationName, operationInput);
        }
    }

    public interface IEntityProxyContext
    {
        Task<TResult> InvokeAsync<TResult>(EntityId entityId, string operationName, object operationInput);
    }

    public class OrchestrationContextProxy : IEntityProxyContext
    {
        public OrchestrationContextProxy(IDurableOrchestrationContext context)
        {
            _context = context;
        }

        private readonly IDurableOrchestrationContext _context;

        public Task<TResult> InvokeAsync<TResult>(EntityId entityId, string operationName, object operationInput)
        {
            return _context.CallEntityAsync<TResult>(entityId, operationName, operationInput);
        }
    }

    public class OrchestrationClientProxy : IEntityProxyContext
    {
        public OrchestrationClientProxy(IDurableOrchestrationClient client)
        {
            _client = client;
        }

        private readonly IDurableOrchestrationClient _client;

        public async Task<TResult> InvokeAsync<TResult>(EntityId entityId, string operationName, object operationInput)
        {
            await _client.SignalEntityAsync(entityId, operationName, operationInput);

            return default;
        }
    }

    public class EntityContextProxy : IEntityProxyContext
    {
        public EntityContextProxy(IDurableEntityContext context)
        {
            _context = context;
        }

        private readonly IDurableEntityContext _context;

        public Task<TResult> InvokeAsync<TResult>(EntityId entityId, string operationName, object operationInput)
        {
            _context.SignalEntity(entityId, operationName, operationInput);

            return Task.FromResult<TResult>(default);
        }
    }
}
