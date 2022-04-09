using System;
using System.Threading.Tasks;

namespace InterviewCode.ProducerConsumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // new PatternMatchingKnuthMorrisPratt().Test();
            await ProducerConsumer.Driver.Test();
        }
    }
}
