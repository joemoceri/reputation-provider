using System;

namespace MefReputationProvider
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var provider = new ReputationProvider())
            {
                var result = provider.GetAll();
                Console.Read();
            }
        }
    }
}
