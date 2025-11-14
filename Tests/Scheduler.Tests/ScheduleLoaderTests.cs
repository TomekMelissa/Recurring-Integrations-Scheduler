using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using Quartz.Spi;
using Quartz.Util;
using Quartz.Xml;
using RecurringIntegrationsScheduler.Tests.TestCommon.Fixtures;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RecurringIntegrationsScheduler.Scheduler.Tests
{
    [TestClass]
    public class ScheduleLoaderTests
    {
        [TestMethod]
        public async Task LoadStandaloneSchedule_DefinesUploadAndDownloadJobs()
        {
            var testDataRoot = FixtureManager.MaterializeTestData("SchedulerTests");
            var schedulePath = Path.Combine(testDataRoot, "Schedules", "Schedule.Standalone.xml");
            FixtureManager.ApplyStandardTokens(schedulePath, testDataRoot, "secret", "password");

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
                var processor = new XMLSchedulingDataProcessor(SimpleTypeLoadHelperFactory());
                processor.ProcessFileAndScheduleJobs(schedulePath, scheduler);

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
                await scheduler.Shutdown();
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
