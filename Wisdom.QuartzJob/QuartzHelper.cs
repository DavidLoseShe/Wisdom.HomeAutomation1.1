namespace Wisdom.QuartzJob
{
    using Quartz;
    using Quartz.Impl;

    namespace Cong.Utility
    {
        public class QuartzHelper
        {
            private static readonly IScheduler Scheduler = GetScheduler();
            private static IScheduler GetScheduler()
            {
                var sf = new StdSchedulerFactory();
                IScheduler scheduler = null;
                try
                {
                    scheduler = sf.GetScheduler();
                }
                catch (SchedulerException e)
                {
                   
                }
                return scheduler;
            }
            public static IScheduler GetInstanceScheduler()
            {
                return Scheduler;
            }
            /// <summary>
            /// 时间间隔执行任务
            /// </summary>
            /// <typeparam name="T">任务类，必须实现IJob接口</typeparam>
            /// <param name="seconds">时间间隔(单位：毫秒)</param>
            /// <param name="address">按钮地址</param>
            public static void ExecuteInterval<T>(int seconds,int address) where T : IJob
            {
                var job = JobBuilder.Create<T>().Build();
                job.JobDataMap.Put("address", address);

                var trigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(seconds).RepeatForever())
                    .Build();

                Scheduler.ScheduleJob(job, trigger);

                Scheduler.Start();
            }

            /// <summary>
            /// 指定时间执行任务
            /// </summary>
            /// <typeparam name="T">任务类，必须实现IJob接口</typeparam>
            /// <param name="cronExpression">cron表达式，即指定时间点的表达式</param>
            /// <param name="address">按钮地址</param>
            /// <param name="start">电器操作地址</param>
            public static void ExecuteByCron<T>(string cronExpression,int address,int start=-1) where T : IJob
            {
                var job = JobBuilder.Create<T>().Build();
                job.JobDataMap.Put("address", address);
                job.JobDataMap.Put("start", start);

                var trigger = (ICronTrigger)TriggerBuilder.Create()
                    .WithCronSchedule(cronExpression)
                    .Build();

                Scheduler.ScheduleJob(job, trigger);
                Scheduler.Start();
            }
        }

        #region 任务执行例
        //public class MyJob : IJob
        //{
        //    public void Execute(IJobExecutionContext context)
        //    {
        //        Console.WriteLine("executed..." + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
        //    }
        //} 
        #endregion
    }
}
