using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Simpl;
using Quartz.Spi;
using Quartz.Util;
using Quartz.Xml;
using RecurringIntegrationsScheduler.Tests.TestCommon.Fixtures;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RecurringIntegrationsScheduler.Scheduler.Tests
{
    [TestClass]
    public class ScheduleLoaderTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public async Task LoadStandaloneSchedule_DefinesUploadAndDownloadJobs()
        {
            var testDataRoot = FixtureManager.MaterializeTestData("SchedulerTests");
            var schedulePath = Path.Combine(testDataRoot, "Schedules", "Schedule.Standalone.xml");
            FixtureManager.ApplyStandardTokens(schedulePath, testDataRoot, "secret", "password");
            Assert.IsTrue(File.Exists(schedulePath), $"Schedule file '{schedulePath}' not found.");
            TestContext?.WriteLine($"Schedule path: {schedulePath}");

            // Ensure job assemblies are loaded into the current AppDomain so Quartz can resolve the job types.
            _ = typeof(RecurringIntegrationsScheduler.Job.Upload);
            _ = typeof(RecurringIntegrationsScheduler.Job.Download);

            var properties = new System.Collections.Specialized.NameValueCollection
            {
                ["quartz.scheduler.instanceName"] = "SchedulerTests",
                ["quartz.jobStore.type"] = "Quartz.Simpl.RAMJobStore, Quartz",
                ["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz",
                ["quartz.threadPool.threadCount"] = "2"
            };

            var factory = new StdSchedulerFactory(properties);
            var scheduler = await factory.GetScheduler();

            try
            {
                var typeLoadHelper = SimpleTypeLoadHelperFactory();
                var processor = new XMLSchedulingDataProcessor(typeLoadHelper);
                await processor.ProcessFileAndScheduleJobs(schedulePath, scheduler).ConfigureAwait(false);

                await scheduler.Start().ConfigureAwait(false);

                var jobGroupNames = await scheduler.GetJobGroupNames();
                TestContext?.WriteLine($"Job groups: {string.Join(", ", jobGroupNames)}");

                var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());
                TestContext?.WriteLine($"Scheduler keys: {string.Join(", ", jobKeys.Select(k => k.ToString()))}");

                var uploadJob = await scheduler.GetJobDetail(new JobKey("Upload.Single", "Upload"));
                Assert.IsNotNull(uploadJob, "Upload job should exist in scheduler");
                Assert.AreEqual("RecurringIntegrationsScheduler.Job.Upload", uploadJob.JobType.FullName);

                var downloadJob = await scheduler.GetJobDetail(new JobKey("Download.Single", "Download"));
                Assert.IsNotNull(downloadJob, "Download job should exist in scheduler");
                Assert.AreEqual("RecurringIntegrationsScheduler.Job.Download", downloadJob.JobType.FullName);

                var uploadInputDir = uploadJob.JobDataMap.GetString("InputDir");
                StringAssert.Contains(uploadInputDir, "FileSystem", "Upload job data map should point to fixture file system.");
            }
            finally
            {
                await scheduler.Shutdown().ConfigureAwait(false);
            }
        }

        private static ITypeLoadHelper SimpleTypeLoadHelperFactory()
        {
            var helper = new SimpleTypeLoadHelper();
            helper.Initialize();
            return helper;
        }
    }
}
