using System;
using System.Collections.Generic;
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
        private readonly List<Task> _incompletedTasks = new List<Task>();

        protected internal Task CallAsync(string operationName, object operationInput)
        {
            return _context.CallAsync(_entityId, operationName, operationInput);
        }

        protected internal Task<TResult> CallAsync<TResult>(string operationName, object operationInput)
        {
            return _context.CallAsync<TResult>(_entityId, operationName, operationInput);
        }

        protected internal void Signal(string operationName, object operationInput)
        {
            _incompletedTasks.Add(_context.Signal(_entityId, operationName, operationInput));
        }

        protected internal Task WaitAllAsync()
        {
            return Task.WhenAll(_incompletedTasks);
        }
    }

    public class EntityProxy<TEntityInterface>
    {
        public EntityProxy(IEntityProxyContext context, EntityId entityId)
        {
            Entity = EntityProxyFactory.Create<TEntityInterface>(context, entityId);
        }

        public TEntityInterface Entity { get; }

        public Task BatchAsync(Action<TEntityInterface> action)
        {
            action(Entity);

            return FlushAsync();
        }

        public Task FlushAsync()
        {
            return (Entity as EntityProxy)?.WaitAllAsync();
        }
    }

    public interface IEntityProxyContext
    {
        Task CallAsync(EntityId entityId, string operationName, object operationInput);
        Task<TResult> CallAsync<TResult>(EntityId entityId, string operationName, object operationInput);
        Task Signal(EntityId entityId, string operationName, object operationInput);
    }

    public class OrchestrationContextProxy : IEntityProxyContext
    {
        public OrchestrationContextProxy(IDurableOrchestrationContext context)
        {
            _context = context;
        }

        private readonly IDurableOrchestrationContext _context;

        public Task CallAsync(EntityId entityId, string operationName, object operationInput)
        {
            return _context.CallEntityAsync(entityId, operationName, operationInput);
        }

        public Task<TResult> CallAsync<TResult>(EntityId entityId, string operationName, object operationInput)
        {
            return _context.CallEntityAsync<TResult>(entityId, operationName, operationInput);
        }

        public Task Signal(EntityId entityId, string operationName, object operationInput)
        {
            _context.SignalEntity(entityId, operationName, operationInput);

            return Task.CompletedTask;
        }
    }

    public class OrchestrationClientProxy : IEntityProxyContext
    {
        public OrchestrationClientProxy(IDurableOrchestrationClient client)
        {
            _client = client;
        }

        private readonly IDurableOrchestrationClient _client;

        public Task CallAsync(EntityId entityId, string operationName, object operationInput)
        {
            throw new NotSupportedException();
        }

        public Task<TResult> CallAsync<TResult>(EntityId entityId, string operationName, object operationInput)
        {
            throw new NotSupportedException();
        }

        public Task Signal(EntityId entityId, string operationName, object operationInput)
        {
            return _client.SignalEntityAsync(entityId, operationName, operationInput);
        }
    }

    public class EntityContextProxy : IEntityProxyContext
    {
        public EntityContextProxy(IDurableEntityContext context)
        {
            _context = context;
        }

        private readonly IDurableEntityContext _context;

        public Task CallAsync(EntityId entityId, string operationName, object operationInput)
        {
            throw new NotSupportedException();
        }

        public Task<TResult> CallAsync<TResult>(EntityId entityId, string operationName, object operationInput)
        {
            throw new NotSupportedException();
        }

        public Task Signal(EntityId entityId, string operationName, object operationInput)
        {
            _context.SignalEntity(entityId, operationName, operationInput);

            return Task.CompletedTask;
        }
    }
}
