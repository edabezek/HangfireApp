using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HangfireApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]//fetch error
    public class SendMailController : ControllerBase
    {
        private readonly IJobTestService _jobTestService;
        private readonly IBackgroundJobClient _backgroundJobClient;//We will be using this interface’s methods for scheduling different types of jobs.
        private readonly IRecurringJobManager _recurringJobManager;

        public SendMailController(IJobTestService jobTestService, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            _jobTestService = jobTestService;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }

        [HttpPost]
        [Route("welcome")]
        public IActionResult Welcome(string userName)
        {
            var jobId =_backgroundJobClient.Enqueue(() => SendWelcomeMail(userName));// stores jobId 
            return Ok($"Job Id {jobId} Completed. Welcome Mail Sent!");
        }

        public void SendWelcomeMail(string userName)
        {
            //Logic to Mail the user
            Console.WriteLine($"Welcome to our application, {userName}");
        }
        [HttpPost]
        [Route("delayedWelcome")]
        public IActionResult DelayedWelcome(string userName)
        {
            var jobId =_backgroundJobClient.Schedule(() => SendDelayedWelcomeMail(userName), TimeSpan.FromMinutes(2));//That means, our job will be executed 2 minutes after the action has been called
            return Ok($"Job Id {jobId} Scheduled. Delayed Welcome Mail will be sent in 2 minutes!");
        }

        public void SendDelayedWelcomeMail(string userName)
        {
            Console.WriteLine($"Welcome to our application, {userName}");
        }
        [HttpPost]
        [Route("invoice")]
        public IActionResult Invoice(string userName)
        {
            _recurringJobManager.AddOrUpdate("jobId", () => SendDelayedWelcomeMail(userName), Cron.Monthly);
            return Ok($"Recurring Job Scheduled. Invoice will be mailed Monthly for {userName}!");
        }

        public void SendInvoiceMail(string userName)
        {
            Console.WriteLine($"Here is your invoice, {userName}");
        }
        [HttpPost]
        [Route("unsubscribe")]
        public IActionResult Unsubscribe(string userName)
        {
            var jobId = _backgroundJobClient.Enqueue(() => UnsubscribeUser(userName));
            _backgroundJobClient.ContinueJobWith(jobId, () => Console.WriteLine($"Sent Confirmation Mail to {userName}"));
            return Ok($"Unsubscribed");
        }

        public void UnsubscribeUser(string userName)
        {
            Console.WriteLine($"Unsubscribed {userName}");
        }
    }
}
