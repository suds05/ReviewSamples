/// <summary>
/// Code samples for your reviewing pleasure.
/// Created by Sudhakar Narayanamurthy, 2020
/// </summary>
namespace Samples
{
    // High performance high concurrency hash table
    // TODO: get even more concurrency and performance!
    using System;

    class HiPerformanceConcurrentHashtable
    {
        private Node[] buckets;
        private volatile bool inUse;
        
        public void Initialize(long size = 10000)
        {
            if (this.buckets == null)
            {
                buckets = new Node[size];
                inUse = false;
            }
        }

        public void Insert(string key, object value)
        {
            bool done = false;
            while (!done)
            {
                if(!inUse)
                {
                    inUse = true;
                    long index = GetHashIndex(key);
                    Node prev = buckets[index];
                    while (prev.Next != null)
                        prev = prev.Next;
                    prev.Next = new Node() { Key = key, Value = value, Next = null };
                    done = true;
                    inUse = false;
                }
                System.Threading.Thread.Sleep(1); // busy wait for high performance.
            }
        }


        public object Lookup(string key)
        {
            long index = GetHashIndex(key);
            Node iNode = buckets[index].Next;
            while (iNode != null && iNode.Key != key)
                iNode = iNode.Next;
            return iNode?.Value;
        }

        private static long GetHashIndex(string key)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            return BitConverter.ToInt64(
                md5.ComputeHash(
                    System.Text.Encoding.UTF8.GetBytes(key)), 0);
        }

        class Node
        {
            public string Key { get; set; }
            public object Value { get; set; }
            public Node Next { get; set; }
        }
    }
}
