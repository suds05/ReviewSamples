using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewCode.ProducerConsumer
{
    interface IConsumer
    {
        Task Consume();

        Task ConsumeAll();
    }

    class Consumer : IConsumer
    {
        IChannel<int> channel;
        private Random random;
        int consumerNo;
        int count;

        public Consumer(int consumerNo, int count, IChannel<int> channel)
        {
            this.channel = channel;
            this.consumerNo = consumerNo;
            this.count = count;
            this.random = new Random(consumerNo);
        }

        public async Task Consume()
        {
            int sleepTime = random.Next(5000, 40000);
            await Task.Delay(sleepTime);
            int item = await this.channel.ConsumeItemFrom();
            Console.WriteLine($"{this.consumerNo}: Consuming {item} after a sleeptime of {sleepTime}");
        }

        public async Task ConsumeAll()
        {
            for(int i = 0; i < this.count; i++)
            {
                await this.Consume();
            }
        }
    }
}
