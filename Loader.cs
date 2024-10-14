
using System;
using System.Threading;

namespace Loaders
{
    public class Loader
    {
        private static bool active = false;
        private static Thread loaderThread;

        public static void Start()
        {
            active = true;
            loaderThread = new Thread(() =>
            {
                Console.Write("Loading...");
                while (active)
                {
                    Thread.Sleep(500);  // Keep sleeping until loading is finished
                }
            });
            loaderThread.Start();
        }

        public static void Stop()
        {
            active = false;
            loaderThread.Join();
            // Remove the "Loading..." message after loading is complete
            Console.WriteLine("\r                 \r"); // Overwrite the "Loading..." with spaces and return cursor to the start
        }
    }
}

