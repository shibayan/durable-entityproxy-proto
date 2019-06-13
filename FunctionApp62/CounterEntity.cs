using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;

using Newtonsoft.Json;

namespace FunctionApp62
{
    public interface ICounterEntity
    {
        void Increment();
        void Add(int value);
        Task<int> Get();
        void Set(int newValue);
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CounterEntity : ICounterEntity
    {
        [FunctionName(nameof(CounterEntity))]
        public async Task Counter([EntityTrigger] IDurableEntityContext context)
        {
            // await context.DispatchAsync<CounterEntity>();

            CurrentValue = context.GetState<int>();

            switch (context.OperationName)
            {
                case nameof(Increment):
                    Increment();
                    break;
                case nameof(Add):
                    Add(context.GetInput<int>());
                    break;
                case nameof(Get):
                    context.Return(await Get());
                    break;
                case nameof(Set):
                    Set(context.GetInput<int>());
                    break;
            }

            context.SetState(CurrentValue);
        }

        [JsonProperty]
        public int CurrentValue { get; set; }

        public void Increment()
        {
            CurrentValue += 1;
        }

        public void Add(int value)
        {
            CurrentValue += value;
        }

        public async Task<int> Get()
        {
            return CurrentValue;
        }

        public void Set(int newValue)
        {
            CurrentValue = newValue;
        }
    }
}