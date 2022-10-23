namespace HangfireApp
{
    public interface IJobTestService//Hangfire job types 
    {
        void FireAndForgetJob();
        void ReccuringJob();
        void DelayedJob();
        void ContinuationJob();
    }
}
