using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;

namespace FunctionApp62
{
    public abstract class EntityProxy<TEntity>
    {
        protected EntityProxy(IDurableOrchestrationContext context, string entityKey)
        {
            _context = context;
            _entityId = new EntityId(typeof(TEntity).Name, entityKey);
        }

        private readonly IDurableOrchestrationContext _context;
        private readonly EntityId _entityId;

        protected Task<TResult> CallEntityAsync<TResult>(string operationName, object operationInput = null)
        {
            return _context.CallEntityAsync<TResult>(_entityId, operationName, operationInput);
        }
    }
}