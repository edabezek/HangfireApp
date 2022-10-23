using System;

namespace HangfireApp
{
    public class JobTestService : IJobTestService
    {
        public void ContinuationJob()
        {
            Console.WriteLine("Hello from a Continuation Job!");
        }

        public void DelayedJob()
        {
            Console.WriteLine("Hello from a Delayed Job!");
        }

        public void FireAndForgetJob()
        {
            Console.WriteLine("Hello from a Fire and Forget Job!");
        }

        public void ReccuringJob()
        {
            Console.WriteLine("Hello from a Scheduled Job!");
        }
    }
}
