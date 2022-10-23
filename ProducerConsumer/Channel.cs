namespace InterviewCode.ProducerConsumer
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Producer-Consumer application
    /// 1. Producers produce items and put it into a channel
    /// 2. Consumers consume items from a channel.
    /// 3. Channel has a finite capacity - say N slots.
    /// 4. If all slots are full, the producers have to be throttled
    /// 5. If all slots are empty, the consumers have to be throttled
    /// 
    /// Out of scope:
    /// Starvation of any individual producer or consumer isn't guaranteed. Focus is to make the system progress
    /// </summary>
    interface IChannel<T>
    {
        Task ProduceItemInto(T item);

        Task<T> ConsumeItemFrom();
    }

    /// <summary>
    /// Channel to coordinate across producers and consumers
    /// </summary>
    /// <typeparam name="T">Type of item</typeparam>
    class Channel<T> : IChannel<T>
    {
        /// <summary>
        /// Buffer to store the items
        /// </summary>
        ConcurrentQueue<T> queue;

        /// <summary>
        /// Semaphore to throttle producers. Producer will wait on this, Consumers will signal this.
        /// </summary>
        SemaphoreSlim emptySlotsSemaphore;

        /// <summary>
        /// Semaphore to throttle consumers. Consumers will wait on this, producers will consume it
        /// </summary>
        SemaphoreSlim itemsSemaphore;

        public Channel(int size)
        {
            queue = new ConcurrentQueue<T>();

            // 10 slots max, initially all empty for producers
            emptySlotsSemaphore = new SemaphoreSlim(size, size);

            // 10 items max, initially no items for consumers
            itemsSemaphore = new SemaphoreSlim(0, size);
        }

        public async Task<T> ConsumeItemFrom()
        {
            // Wait for some item to be present
            await itemsSemaphore.WaitAsync();

            // Pull out the item
            T result;
            while (!queue.TryDequeue(out result))
            {
                await Task.Delay(1000);
            }

            // Signal that empty slots are present
            emptySlotsSemaphore.Release();

            return result;
        }

        public async Task ProduceItemInto(T item)
        {
            // Wait for some empty slot
            await emptySlotsSemaphore.WaitAsync();

            // Push the item in
            queue.Enqueue(item);

            // Signal that items are now present
            itemsSemaphore.Release();
        }
    }
}
