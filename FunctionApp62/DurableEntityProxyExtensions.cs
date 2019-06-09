using Microsoft.Azure.WebJobs;

namespace FunctionApp62
{
    public static class DurableEntityProxyExtensions
    {
        public static TEntityInterface CreateEntityProxy<TEntityInterface>(this IDurableOrchestrationClient client, EntityId entityId)
        {
            return EntityProxyFactory.Create<TEntityInterface>(new OrchestrationClientProxy(client), entityId);
        }

        public static TEntityInterface CreateEntityProxy<TEntityInterface>(this IDurableOrchestrationContext context, EntityId entityId)
        {
            return EntityProxyFactory.Create<TEntityInterface>(new OrchestrationContextProxy(context), entityId);
        }

        public static TEntityInterface CreateEntityProxy<TEntityInterface>(this IDurableEntityContext context, EntityId entityId)
        {
            return EntityProxyFactory.Create<TEntityInterface>(new EntityContextProxy(context), entityId);
        }
    }
}