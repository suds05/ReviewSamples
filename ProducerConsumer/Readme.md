# Producer Consumer application

## Basic idea
1. Producers produce items and put it into a channel
2. Consumers consume items from a channel.
3. Channel has a finite capacity - say N slots.
4. If all slots are full, the producers have to be throttled
5. If all slots are empty, the consumers have to be throttled
    
## Out of scope
Starvation of any individual producer or consumer isn't guaranteed. Focus is to make the system progress

## Approach
1. Use semaphores to coordinate across producers and consumers access to the buffer, also called a channel.
2. The 'Slots' semaphore indicates empty slots. Producers wait on this, while consumers signal it.
3. The 'Items' semaphore indicates produced items. Consumers wait on this, while producers signal it.
4. All application written asynchronously using Tasks, so that things that can run concurrently can do.