using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewCode.ProducerConsumer
{
    class Driver
    {
        const int itemsPerEntity = 2;
        const int noOfEntities = 2;

        public static async Task Test()
        {
            var channel = new Channel<int>(itemsPerEntity);

            Task[] producerTasks = RunProducers(channel);

            Task[] consumerTasks = RunConsumers(channel);

            await Task.WhenAll(producerTasks);

            await Task.WhenAll(consumerTasks);
        }

        static Task[] RunProducers(IChannel<int> channel)
        {
            return Enumerable.Range(0, noOfEntities)
               .Select<int, Task>(
                    (int no) => (new SleepyProducer(no * itemsPerEntity, itemsPerEntity, channel)).ProduceAll())
               .ToArray();
        }

        static Task[] RunConsumers(IChannel<int> channel)
        {
            return Enumerable.Range(0, noOfEntities)
               .Select<int, Task>(
                    (int no) => (new Consumer(no * itemsPerEntity, itemsPerEntity, channel)).ConsumeAll())
               .ToArray();
        }
    }
}
