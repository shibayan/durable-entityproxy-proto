using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;

using Newtonsoft.Json;

namespace FunctionApp62
{
    public interface ICounterEntity
    {
        Task<int> Add(int value);
        Task<int> Sub(int value);
        Task<int> Reset();
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CounterEntity : ICounterEntity
    {
        [FunctionName(nameof(CounterEntity))]
        public async Task Counter([EntityTrigger(EntityName = nameof(ICounterEntity))]
                                  IDurableEntityContext context)
        {
            // await context.DispatchAsync<CounterEntity>();

            var currentValue = context.GetState<int>();
            int value = context.GetInput<int>();

            switch (context.OperationName)
            {
                case "Add":
                    currentValue += value;
                    break;
                case "Sub":
                    currentValue -= value;
                    break;
                case "Reset":
                    currentValue = 0;
                    break;
            }

            context.SetState(currentValue);
            context.Return(currentValue);
        }

        [JsonProperty]
        public int CurrentValue { get; set; }

        public Task<int> Add(int value)
        {
            return Task.FromResult(CurrentValue += value);
        }

        public Task<int> Sub(int value)
        {
            return Task.FromResult(CurrentValue -= value);
        }

        public Task<int> Reset()
        {
            return Task.FromResult(CurrentValue = 0);
        }
    }
}