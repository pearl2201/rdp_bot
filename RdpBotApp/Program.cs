using RdpBotApp;
using Quartz;
using Quartz.AspNetCore;
using RdpBotApp.Jobs;
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddTransient<DailyEatJob>();
builder.Services.AddTransient<DailyMeetJob>();
builder.Services.AddTransient<DailyLogJiraJob>();
builder.Services.AddTransient<TestJob>();
builder.Services.AddQuartz(q =>
{
    // base quartz scheduler, job and trigger configuration

    // handy when part of cluster or you want to otherwise identify multiple schedulers
    q.SchedulerId = "Scheduler-Core";

    // we take this from appsettings.json, just show it's possible
    // q.SchedulerName = "Quartz ASP.NET Core Sample Scheduler";

    // as of 3.3.2 this also injects scoped services (like EF DbContext) without problems
    q.UseMicrosoftDependencyInjectionJobFactory();

    // or for scoped service support like EF Core DbContext
    // q.UseMicrosoftDependencyInjectionScopedJobFactory();

    // these are the defaults
    q.UseSimpleTypeLoader();

    q.UseDefaultThreadPool(tp =>
    {
        tp.MaxConcurrency = 10;
    });

    //q.ScheduleJob<DailyEatJob>(trigger =>
    //    trigger
    //    .WithIdentity("DailyEatingJob_every_minutes", "daily")
    //    .WithCronSchedule("0 45 3 ? * MON-FRI *", x => x.InTimeZone(TimeZoneInfo.Utc))
    //    .WithDescription("DailyEatingJob")

    //);

    q.ScheduleJob<DailyMeetJob>(trigger =>
        trigger
        .WithIdentity("DailyMeetingJob_every_days", "daily")
        .WithCronSchedule("0 44 1 ? * MON-FRI *", x => x.InTimeZone(TimeZoneInfo.Utc))
        .WithDescription("DailyMeetingJob")

    );

    q.ScheduleJob<DailyLogJiraJob>(trigger =>
        trigger
        .WithIdentity("DailyLogJiraJob_every_days", "daily")
        .WithCronSchedule("0 30 10 ? * MON-FRI *", x => x.InTimeZone(TimeZoneInfo.Utc))
        .WithDescription("DailyLogJiraJob")

    );

    q.ScheduleJob<TestJob>(trigger =>
        trigger
        .WithIdentity("Test job every minute", "daily")
        .WithCronSchedule("0 0/1 * 1/1 * ? *", x => x.InTimeZone(TimeZoneInfo.Utc))
        .WithDescription("TestJobEveryMinute")
    );

});

// ASP.NET Core hosting
builder.Services.AddQuartzServer(options =>
{
    // when shutting down we want jobs to complete gracefully
    options.WaitForJobsToComplete = true;
});

builder.Services.AddScoped<TelegramHttpClient>();
var host = builder.Build();
host.Run();
