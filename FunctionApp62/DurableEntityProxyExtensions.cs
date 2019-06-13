using Microsoft.Azure.WebJobs;

namespace FunctionApp62
{
    public static class DurableEntityProxyExtensions
    {
        public static EntityProxy<TEntityInterface> CreateEntityProxy<TEntityInterface>(this IDurableOrchestrationClient client, EntityId entityId)
        {
            return new EntityProxy<TEntityInterface>(new OrchestrationClientProxy(client), entityId);
        }

        public static EntityProxy<TEntityInterface> CreateEntityProxy<TEntityInterface>(this IDurableOrchestrationContext context, EntityId entityId)
        {
            return new EntityProxy<TEntityInterface>(new OrchestrationContextProxy(context), entityId);
        }

        public static EntityProxy<TEntityInterface> CreateEntityProxy<TEntityInterface>(this IDurableEntityContext context, EntityId entityId)
        {
            return new EntityProxy<TEntityInterface>(new EntityContextProxy(context), entityId);
        }
    }
}