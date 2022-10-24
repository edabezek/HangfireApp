# HangfireApp Notes

## Background Tasks

To keep our applications running smoothly, for certain tasks we can utilize a concept called background processing. A task is a method in
our code with some logic that we need to execute. Processing it in the background means that we can execute it away from the application’s 
main thread.

Structures that allow a process to do more than one job at the same time are called threads.

A process can contain one or more threads. Threads can only do one job at a time.

Running more than one thread in a process is called multi-threading.

Running programs are called processes. 

For example, Word is a program while Excel or any other application is not running yet. Programs are called processes when they are run.Processes start their lives with a single thread and this thread is called main thread. Other threads are created by system functions during 
program execution.

While processes are running in isolation, threads share the same memory resource.

If a process is blocked, another process cannot perform its operation. First, it waits for the first process to finish its job. However, 
this does not apply to threads. A thread can run even if another thread is blocked.

The default thread size for Windows is 1 MB. 

Threads can terminate in many different ways. If the function executed by the thread is completed,the thread is terminated. At the same time,
if the main thread terminates, all the threads of the process are terminated.


## Hangfire

Open source task schedulers,uses persistent storage and it supports multiple queue processing .

Persistent storage saves the job state, we also have a job retries. This feature helps make sure our jobs finish executing even 
if they run into a transient exception or if the dedicated application pool crashes. If a job fails, Hangfire will try to run it again as soon as 
possible.

## Security

Since the dashboard may expose very sensitive data like method names, parameter values, email-ids, it is highly important that we secure/restrict
this endpoint. Hangfire, out of the box makes the dashboard secure by allowing only local requests. We can change this by implementing  of 
IDashboardAuthorizationFilter.
	
## Components
	
	Client : Client creates job
	Storage : This our db.It stores all the information about our jobs.Default its uses SQLServer.
	Server : The server has the task of picking up job definitions from the storage and executing them. It’s also responsible for keeping our job
storage clean from any data that we don’t use anymore. The server can live within our application, or it can be on another server. It always
points to the database storage. 

## Workflow

After we specify our task in the code and call the appropriate Hangfire method to create it, the Hangfire client creates the job and stores it
in the database.

The control then returns to the application so the main thread can continue with its work.When a job is in the storage, the server picks it 
up and creates a background thread to process the fetched job.
	
## Dashboard

Within the dashboard, we can see all the running and scheduled jobs that we create with our Hangfire client. We can also monitor servers,
job retries, failed jobs and keep an eye on all jobs in the queue. Another great thing that we can do in the dashboard is we can manually trigger 
any existing jobs.

## Servers

Here the server running in the test application appears.
If we had more server instances running, we would also see them listed here. Hangfire checks server usage periodically,so if there is a server
that is not in use anymore, it will be automatically removed from the list.

# Job Types

## Fire And Forget Jobs

	This jobs are executed only once and almost immediately after creation.


## Succeeded Jobs

	Successful tests appear here and the related job can be run again with the Requeue button.

## Delayed Jobs 

	For jobs we want to run in the future.We can schedule them at a certain time.

## Reccuring Jobs / Repeating
	
	They can repeat in a certain interval.To schedule this job we will need a different Hangfire interface is IRecurringJobManager.
  
## Continuation Jobs

	Its main feature is that it chains together task execution. With it, we can get two jobs to run one after the other in continuation.


### Packages :
* Hangfire 1.7.22

* Hangfire.SqlServer 1.7.22

* Hangfire.Core 1.7.22

