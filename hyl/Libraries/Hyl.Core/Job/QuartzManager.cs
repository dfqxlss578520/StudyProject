using System;
using Hyl.Core.Infrastructure;
using Hyl.Core.Logs;
using Quartz;
using Quartz.Impl;

namespace Hyl.Core.Job
{
    public class QuartzManager : IJobManager
    {
        private readonly ILogs<QuartzManager> _log;
        public QuartzManager(ILogs<QuartzManager> log)
        {
            _log = log;
        }
        private ISchedulerFactory scheduleFactory = new StdSchedulerFactory();
        /// <summary>  
        /// 添加定时任务  
        /// </summary>  
        /// <typeparam name="T">Job类型</typeparam>  
        /// <param name="jobName"></param>  
        /// <param name="jobGroup"></param>  
        /// <param name="dataMap"></param>  
        /// <param name="timeExp">Cron时间表达式</param>  
        /// <returns></returns>  
        public bool AddJob<T>(string jobName, string jobGroup, string timeExp) where T : IJob
        {
            try
            {
                IScheduler schedule = scheduleFactory.GetScheduler();
                //新建JobDetail  
                IJobDetail jobDetail = JobBuilder.Create<T>().WithIdentity(jobName, jobGroup).Build();
                //新建trigger  
                ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                    .WithIdentity(jobName, jobGroup)
                    .WithCronSchedule(timeExp)
                    .Build();
                schedule.ScheduleJob(jobDetail, trigger);
                return true;
            }
            catch (Exception ex)
            {
                //log:添加任务失败+jobName:jobName+jobGroup:jobGroup+ex.message  
                return false;
            }
        }
        /// <summary>  
        /// 添加定时任务  
        /// </summary>  
        /// <typeparam name="T">Job类型</typeparam>  
        /// <param name="jobName"></param>  
        /// <param name="jobGroup"></param>  
        /// <param name="dataMap"></param>  
        /// <param name="timeExp">Cron时间表达式</param>  
        /// <returns></returns>  
        public bool AddJob<T>(string jobName, string jobGroup, JobDataMap dataMap, string timeExp) where T : IJob
        {
            try
            {
                IScheduler schedule = scheduleFactory.GetScheduler();
                //新建JobDetail  
                IJobDetail jobDetail = JobBuilder.Create<T>().WithIdentity(jobName, jobGroup).SetJobData(dataMap).Build();
                //新建trigger  
                ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                    .WithIdentity(jobName, jobGroup)
                    .WithCronSchedule(timeExp)
                    .Build();
                schedule.ScheduleJob(jobDetail, trigger);
                return true;
            }
            catch (Exception ex)
            {
                //log:添加任务失败+jobName:jobName+jobGroup:jobGroup+ex.message  
                return false;
            }
        }
        /// <summary>  
        /// 添加Job  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="jobName"></param>  
        /// <param name="jobGroup"></param>  
        /// <param name="dataMap"></param>  
        /// <param name="triggerName"></param>  
        /// <param name="triggerGroup"></param>  
        /// <param name="timeExp"></param>  
        /// <returns></returns>  
        public bool AddJob<T>(string jobName, string jobGroup, JobDataMap dataMap, string triggerName, string triggerGroup, string timeExp) where T : IJob
        {
            try
            {
                IScheduler schedule = scheduleFactory.GetScheduler();
                IJobDetail jobDetail = JobBuilder.Create<T>().WithIdentity(jobName, jobGroup).SetJobData(dataMap).Build();
                ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                    .WithIdentity(triggerName, triggerGroup)
                    .WithCronSchedule(timeExp)
                    .Build();
                schedule.ScheduleJob(jobDetail, trigger);
                return true;
            }
            catch (Exception ex)
            {
                //log:添加任务失败+jobName:jobName+jobGroup:jobGroup+ex.message  
                return false;
            }
        }
        /// <summary>  
        /// 修改Job时间  
        /// </summary>  
        /// <param name="triggerName"></param>  
        /// <param name="triggerGroup"></param>  
        /// <param name="timeExp"></param>  
        /// <returns></returns>  
        public bool ModifyJobTime(string triggerName, string triggerGroup, string timeExp)
        {
            try
            {
                IScheduler schedule = scheduleFactory.GetScheduler();
                ICronTrigger trigger = (ICronTrigger)schedule.GetTrigger(new TriggerKey(triggerName, triggerGroup));
                if (trigger == null)
                {
                    //log:未找到触发器triggerName:triggerName+triggerGroup:triggerGroup  
                    return false;
                }
                string oldCron = trigger.CronExpressionString;
                string newCron = timeExp;
                if (oldCron != newCron)
                {
                    schedule.PauseTrigger(new TriggerKey(triggerName, triggerGroup));
                    trigger.CronExpressionString = newCron;
                    schedule.RescheduleJob(new TriggerKey(triggerName, triggerGroup), trigger);
                    // schedule.ResumeTrigger(new TriggerKey(triggerName, triggerGroup));  
                }
                return true;
            }
            catch (Exception ex)
            {
                //log:修改任务失败+triggerName:triggerName+triggerGroup:triggerGroup+ex.message  
                return false;
            }
        }

        ///// <summary>  
        ///// 修改Job内容  
        ///// </summary>  
        ///// <param name="jobName"></param>  
        ///// <param name="jobGroup"></param>  
        ///// <param name="triggerName"></param>  
        ///// <param name="triggerGroup"></param>  
        ///// <param name="dataMap"></param>  
        ///// <returns></returns>  
        //public static bool ModifyJobDataMap(string jobName, string jobGroup, string triggerName, string triggerGroup, JobDataMap dataMap)
        //{
        //    try
        //    {
        //        #region   
        //        IScheduler schedule = scheduleFactory.GetScheduler();
        //        ICronTrigger trigger = (ICronTrigger)schedule.GetTrigger(new TriggerKey(triggerName, triggerGroup));
        //        if (trigger == null)
        //        {
        //            //log:未找到triggerName:triggerName+triggerGroup:triggerGroup  
        //            return false;
        //        }
        //        string Cron = trigger.CronExpressionString;
        //        DeleteJob(jobName, jobGroup, triggerName, triggerGroup);
        //        AddJob<Job.ForJob>(jobName, jobGroup, dataMap, triggerName, triggerGroup, Cron);
        //        #endregion
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //log:修改任务内容失败triggerName:triggerName+triggerGroup:triggerGroup+ex.message  
        //        return false;
        //    }
        //}

        /// <summary>  
        /// 暂停任务  
        /// </summary>  
        /// <param name="jobName"></param>  
        /// <param name="jobGroup"></param>  
        /// <returns></returns>  
        public bool PauseJob(string jobName, string jobGroup)
        {
            try
            {
                IScheduler schedule = scheduleFactory.GetScheduler();
                schedule.PauseTrigger(new TriggerKey(jobName, jobGroup));  //停止触发器  
                schedule.UnscheduleJob(new TriggerKey(jobName, jobGroup)); //删除触发器  
                schedule.PauseJob(new JobKey(jobName, jobGroup)); //删除任务  
                return true;
            }
            catch (Exception ex)
            {
                //log:暂停任务失败+triggerName:triggerName+triggerGroup:triggerGroup+ex.message  
                return false;
            }
        }

        /// <summary>  
        /// 恢复执行任务  
        /// </summary>  
        /// <param name="jobName"></param>  
        /// <param name="jobGroup"></param>  
        /// <returns></returns>  
        public bool ResumeJob(string jobName, string jobGroup)
        {
            try
            {
                IScheduler schedule = scheduleFactory.GetScheduler();
                schedule.PauseTrigger(new TriggerKey(jobName, jobGroup));  //停止触发器  
                schedule.UnscheduleJob(new TriggerKey(jobName, jobGroup)); //删除触发器  
                schedule.ResumeJob(new JobKey(jobName, jobGroup)); //删除任务  
                return true;
            }
            catch (Exception ex)
            {
                //log:恢复执行任务失败+triggerName:triggerName+triggerGroup:triggerGroup+ex.message  
                return false;
            }
        }

        /// <summary>  
        /// 删除任务  
        /// </summary>  
        /// <param name="jobName"></param>  
        /// <param name="jobGroup"></param>  
        /// <returns></returns>  
        public bool DeleteJob(string jobName, string jobGroup)
        {
            try
            {
                IScheduler schedule = scheduleFactory.GetScheduler();
                schedule.PauseTrigger(new TriggerKey(jobName, jobGroup));  //停止触发器  
                schedule.UnscheduleJob(new TriggerKey(jobName, jobGroup)); //删除触发器  
                schedule.DeleteJob(new JobKey(jobName, jobGroup)); //删除任务  
                return true;
            }
            catch (Exception ex)
            {
                //log:删除任务失败+triggerName:triggerName+triggerGroup:triggerGroup+ex.message  
                return false;
            }
        }
        /// <summary>  
        /// 删除任务  
        /// </summary>  
        /// <param name="jobName"></param>  
        /// <param name="jobGroup"></param>  
        /// <param name="triggerName"></param>  
        /// <param name="triggerGroup"></param>  
        /// <returns></returns>  
        public bool DeleteJob(string jobName, string jobGroup, string triggerName, string triggerGroup)
        {
            try
            {
                IScheduler schedule = scheduleFactory.GetScheduler();
                schedule.PauseTrigger(new TriggerKey(triggerName, triggerGroup));  //停止触发器  
                schedule.UnscheduleJob(new TriggerKey(triggerName, triggerGroup)); //删除触发器  
                schedule.DeleteJob(new JobKey(jobName, jobGroup)); //删除任务  
                return true;
            }
            catch (Exception ex)
            {
                //log:删除任务失败+triggerName:triggerName+triggerGroup:triggerGroup+ex.message  
                return false;
            }
        }
        /// <summary>  
        /// 启动所有任务  
        /// </summary>  
        /// <returns></returns>  
        public bool StartAllJob()
        {
            try
            {
                IScheduler sched = EngineContext.Current.Resolve<IScheduler>();
                sched.JobFactory = new AutofacJobFactory(EngineContext.ContainerManager.Container);
                sched.Start();

                //IScheduler schedule = scheduleFactory.GetScheduler();
                //schedule.Start();
                return true;
            }
            catch (Exception ex)
            {
                //log:启动任务失败+ex.message  
                return false;
            }
        }
        /// <summary>  
        /// 关闭所有任务  
        /// </summary>  
        /// <returns></returns>  
        public bool ShutDownJobs()
        {
            try
            {
                IScheduler schedule = scheduleFactory.GetScheduler();
                if (!schedule.IsShutdown)
                {
                    schedule.Shutdown();
                }
                return true;
            }
            catch (Exception ex)
            {
                //log:关闭任务失败+ex.message  
                return false;
            }
        }
    }
}
