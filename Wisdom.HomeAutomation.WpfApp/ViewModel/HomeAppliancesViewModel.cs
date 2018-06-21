using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Wisdom.HomeAutomation.Dal;
using Wisdom.HomeAutomation.Rs485Drivers;

namespace Wisdom.HomeAutomation.WpfApp.ViewModel
{
    public class HomeAppliancesViewModel : ViewModelBase
    {
        private List<t_HomeAppliances> _homeAppliances;

        public HomeAppliancesViewModel()
        {
            
            #region 开关设备
            OpenCloseCommand = new RelayCommand<CheckBox>((checkBox) =>
            {
//                var driver = new Rs485Driver();
//                driver.InstallRelayOutputState(int.Parse(checkBox.Tag.ToString()), checkBox.IsChecked == true);
//                driver.Disconnect();
            });

            #endregion
            #region 设备控制
            ControllerCommand = new RelayCommand<CheckBox>((checkBox) =>
            {
//                var driver = new Rs485Driver();
//                if (checkBox.Tag.ToString() == "15")
//                {
//                    if (checkBox.IsChecked == true)
//                    {
//                        //开关间隔0.5s
//                        driver.InstallRelayOutputState(int.Parse(checkBox.Tag.ToString()), true);
//                        Thread.Sleep(500);
//                        driver.InstallRelayOutputState(int.Parse(checkBox.Tag.ToString()), false);
//                        driver.Disconnect();
//                        return;
//                    }
//                    //开关间隔大于1s
//                    driver.InstallRelayOutputState(int.Parse(checkBox.Tag.ToString()), true);
//                    Thread.Sleep(1500);
//                    driver.InstallRelayOutputState(int.Parse(checkBox.Tag.ToString()), false);
//                    driver.Disconnect();
//                    return;
//                }
//                driver.InstallRelayOutputState(int.Parse(checkBox.Tag.ToString()), checkBox.IsChecked == true);
//                driver.Disconnect();
            });
            #endregion
        }

        public List<t_HomeAppliances> HomeAppliances
        {
            get { return _homeAppliances; }
            set { Set(() => HomeAppliances, ref _homeAppliances, value); }
        }

        /// <summary>
        /// 电器电源控制
        /// </summary>
        public ICommand OpenCloseCommand { get; }

        /// <summary>
        /// 电器命令控制
        /// </summary>
        public ICommand ControllerCommand { get; }
    }
}