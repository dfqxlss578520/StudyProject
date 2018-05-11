using System;
using Hyl.Core.Domain.JobsDomain;
using Hyl.Service.SiteManagers;
using Quartz;

namespace Hyl.Service.JobManager
{
    public class TestJob:IJob
    {
        private readonly ITomJobServices _tomJobServices;

        public TestJob(ITomJobServices tomJobServices)
        {
            _tomJobServices = tomJobServices;
        }
        public void Execute(IJobExecutionContext context)
        {
            _tomJobServices.Insert(new TomJob()
            {
                CONTENT = "JobRunTime：" + context.JobRunTime,
                createDate = DateTime.Now
            });
        }
    }
}
