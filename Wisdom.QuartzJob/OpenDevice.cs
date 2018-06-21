using System;
using System.Threading;
using Quartz;
using Wisdom.HomeAutomation.Rs485Drivers;
using Wisdom.Utils.Driver.Arg;

namespace Wisdom.QuartzJob
{
    /// <summary>
    /// 打开设备
    /// </summary>
    public class OpenDevice : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var driver = new Rs485Driver();
            driver.InstallRelayOutputState(context.JobDetail.JobDataMap.GetIntValue("address"), true);
            driver.Disconnect();
        }
    }
    /// <summary>
    /// 关闭设备
    /// </summary>
    public class CloseDevice : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var driver = new Rs485Driver();
            driver.InstallRelayOutputState(context.JobDetail.JobDataMap.GetIntValue("address"), false);
            driver.Disconnect();
        }
    }
    /// <summary>
    /// 控制开关，确保设备打开
    /// </summary>
    public class StartAndOpenDevice : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var driver = new Rs485Driver();
            driver.InstallRelayOutputState(context.JobDetail.JobDataMap.GetIntValue("address"), true);
            Thread.Sleep(5000);
            driver.InstallRelayOutputState(context.JobDetail.JobDataMap.GetIntValue("start"), true);
            driver.Disconnect();
        }
    }
    /// <summary>
    /// 断开控制开关
    /// </summary>
    public class EndAndOpenDevice : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var driver = new Rs485Driver();
            driver.InstallRelayOutputState(context.JobDetail.JobDataMap.GetIntValue("start"), false);
            driver.Disconnect();
        }
    }
    /// <summary>
    /// 瞬间点动开关
    /// </summary>
    public class ShortOpenCloseDevice : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var driver = new Rs485Driver();
            driver.InstallRelayOutputState(context.JobDetail.JobDataMap.GetIntValue("address"), true);
            Thread.Sleep(5000);
            driver.InstallRelayOutputState(context.JobDetail.JobDataMap.GetIntValue("start"), true);
            Thread.Sleep(500);
            driver.InstallRelayOutputState(context.JobDetail.JobDataMap.GetIntValue("start"), false);
            driver.Disconnect();
        }
    }
    /// <summary>
    /// 长点动开关
    /// </summary>
    public class LongOpenCloseDevice : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var driver = new Rs485Driver();
            driver.InstallRelayOutputState(context.JobDetail.JobDataMap.GetIntValue("start"), true);
            Thread.Sleep(1500);
            driver.InstallRelayOutputState(context.JobDetail.JobDataMap.GetIntValue("start"), false);
            driver.Disconnect();
        }
    }
}