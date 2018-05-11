using Quartz;

namespace Hyl.Core.Job
{
    public interface IJobManager
    {
        /// <summary>  
        /// 添加定时任务  
        /// </summary>  
        /// <typeparam name="T">Job类型</typeparam>  
        /// <param name="jobName"></param>  
        /// <param name="jobGroup"></param>  
        /// <param name="dataMap"></param>  
        /// <param name="timeExp">Cron时间表达式</param>  
        /// <returns></returns>
        bool AddJob<T>(string jobName, string jobGroup, string timeExp) where T : IJob;
        /// <summary>  
        /// 添加定时任务  
        /// </summary>  
        /// <typeparam name="T">Job类型</typeparam>  
        /// <param name="jobName"></param>  
        /// <param name="jobGroup"></param>  
        /// <param name="dataMap"></param>  
        /// <param name="timeExp">Cron时间表达式</param>  
        /// <returns></returns>  
         bool AddJob<T>(string jobName, string jobGroup, JobDataMap dataMap, string timeExp) where T : IJob;

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
         bool AddJob<T>(string jobName, string jobGroup, JobDataMap dataMap, string triggerName,
            string triggerGroup, string timeExp) where T : IJob;

        /// <summary>  
        /// 修改Job时间  
        /// </summary>  
        /// <param name="triggerName"></param>  
        /// <param name="triggerGroup"></param>  
        /// <param name="timeExp"></param>  
        /// <returns></returns>  
         bool ModifyJobTime(string triggerName, string triggerGroup, string timeExp);
        
        /// <summary>  
        /// 暂停任务  
        /// </summary>  
        /// <param name="jobName"></param>  
        /// <param name="jobGroup"></param>  
        /// <returns></returns>  
         bool PauseJob(string jobName, string jobGroup);

        /// <summary>  
        /// 恢复执行任务  
        /// </summary>  
        /// <param name="jobName"></param>  
        /// <param name="jobGroup"></param>  
        /// <returns></returns>  
         bool ResumeJob(string jobName, string jobGroup);
        /// <summary>  
        /// 删除任务  
        /// </summary>  
        /// <param name="jobName"></param>  
        /// <param name="jobGroup"></param>  
        /// <returns></returns>  
         bool DeleteJob(string jobName, string jobGroup);

        /// <summary>  
        /// 启动所有任务  
        /// </summary>  
        /// <returns></returns>  
        bool StartAllJob();

        /// <summary>  
        /// 关闭所有任务  
        /// </summary>  
        /// <returns></returns>  
        bool ShutDownJobs();
    }
}
