using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HangfireApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTestController : ControllerBase
    {
        private readonly IJobTestService _jobTestService;
        private readonly IBackgroundJobClient _backgroundJobClient;//We will be using this interface’s methods for scheduling different types of jobs.
        private readonly IRecurringJobManager _recurringJobManager;
        public JobTestController(IJobTestService jobTestService, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            _jobTestService = jobTestService;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }

        //endpoints

        [HttpGet("/FireAndForgetJob")]
        public ActionResult CreateFireAndForgetJob()
        {
            //Enqueue : kuyruğa alma
            _backgroundJobClient.Enqueue(() => _jobTestService.FireAndForgetJob());//burası job'ı get ile kuyruğa alacak
            return Ok();
        }
        [HttpGet("/DelayedJob")]
        public ActionResult CreateDelayedJob()
        {
            _backgroundJobClient.Schedule(() => _jobTestService.DelayedJob(), TimeSpan.FromSeconds(60));//will create the job definition and save it, but it will also make sure to schedule it in the queue at the specified time
            return Ok();
        }
        [HttpGet("/ReccuringJob")]
        public ActionResult CreateReccuringJob()
        {
            _recurringJobManager.AddOrUpdate("JobId", () => _jobTestService.ReccuringJob(), Cron.Minutely);//The hangfire method will create a new job with the specified id, or update an existing one. In this example, we will just be creating the job for the first time.
            return Ok();
        }
        [HttpGet("/ContinuationJob")]
        public ActionResult CreateContinuationJob()
        {
            var parentJobId = _backgroundJobClient.Enqueue(() => _jobTestService.FireAndForgetJob());//We want this job to be a trigger for the next job, so we make sure to collect the id that the Enqueue() method returns. 
            _backgroundJobClient.ContinueJobWith(parentJobId, () => _jobTestService.ContinuationJob());//After it’s created, we call the interface’s ContinueJobWith() method and pass the job id of the created job along with our ContinuationJob() method call. The ContinueJobWith() method will make sure to chain our two jobs together.
            return Ok();
        }
    }
}
