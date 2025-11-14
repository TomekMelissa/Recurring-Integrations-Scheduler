using Moq;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using SchedulerContracts = RecurringIntegrationsScheduler.Common.Contracts.SchedulerConstants;
using System;

namespace RecurringIntegrationsScheduler.Tests.TestCommon.Quartz
{
    /// <summary>
    /// Utility helpers for building test-friendly Quartz contexts without spinning real schedulers.
    /// </summary>
    public static class TestSchedulerBuilder
    {
        public static IJobExecutionContext CreateContext<TJob>(TJob jobInstance, JobDataMap dataMap) where TJob : IJob
        {
            var jobDetail = JobBuilder.Create<TJob>()
                .WithIdentity("TestJob", "Tests")
                .UsingJobData(dataMap)
                .Build();

            var trigger = (IOperableTrigger)TriggerBuilder.Create()
                .WithIdentity("TestTrigger", "Tests")
                .StartNow()
                .Build();

            var bundle = new TriggerFiredBundle(jobDetail, trigger, null, false, DateTimeOffset.UtcNow, null, null, null);
            var scheduler = new Mock<IScheduler>();
            scheduler.SetupGet(s => s.SchedulerName).Returns("RIS.UnitTests");
            return new JobExecutionContextImpl(scheduler.Object, bundle, jobInstance);
        }
    }
}
