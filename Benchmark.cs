using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace wpa2psk_secure_password_generator
{
    class Benchmark
    {
        public static void Run()
        {
            // Measurement variables
            Stopwatch sw = new Stopwatch();
            int trials = 1000000;
            int maxLength = 63;

            // Dummy run to avoid cold start measurements during trials
            for (int j = 0; j < trials; j++)
            {
                Vault.GenerateWifiPassword(1);
            }

            // Determine average run time for password lengths 1 through 63
            for (int i = 1; i <= maxLength; i++)
            {
                double totalMilliseconds = 0;
                for (int j = 0; j < trials; j++)
                {
                    sw.Restart();
                    Vault.GenerateWifiPassword(i);
                    sw.Stop();
                    totalMilliseconds += sw.Elapsed.TotalMilliseconds;
                }
                double avgMilliseconds = totalMilliseconds / trials;

                Console.WriteLine("Length {0}: Average={1}", i, avgMilliseconds);
            }
        }
    }
}
