using System;
using System.Threading.Tasks;

namespace InterviewCode.ProducerConsumer
{
    interface IProducer
    {
        /// <summary>
        /// Produce one item
        /// </summary>
        /// <returns>return true if done</returns>
        Task<bool> Produce();

        /// <summary>
        /// Create all items
        /// </summary>
        /// <returns></returns>
        Task ProduceAll();
    }
    
    class SleepyProducer : IProducer
    {
        public int start;

        public int count;

        private int lastProduced;

        private Random random;

        private IChannel<int> channel;

        public SleepyProducer(int start, int count, IChannel<int> channel)
        {
            this.start = start;
            this.count = count;
            this.lastProduced = this.start - 1;
            this.channel = channel;
            random = new Random(this.start);
        }

        public async Task<bool> Produce()
        {
            if (this.lastProduced == this.start + this.count - 1)
                return true;

            int sleepTime = random.Next(1, 10);
            await Task.Delay(sleepTime);
            this.lastProduced++;
            await channel.ProduceItemInto(this.lastProduced);
            Console.WriteLine($"{this.start}: Produced {this.lastProduced} after a sleeptime of {sleepTime}");
            return false;
        }

        public async Task ProduceAll()
        {
            bool done = false;
            do
            {
                done = await this.Produce();
            }
            while (!done);
        }
    }

}
