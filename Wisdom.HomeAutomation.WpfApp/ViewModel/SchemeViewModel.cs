using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AgileToDo;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Wisdom.HomeAutomation.Dal;
using Wisdom.HomeAutomation.Rs485Drivers;
using Wisdom.HomeAutomation.WpfApp.Controller;
using Wisdom.Utils;
using Wisdom.Utils.Driver.Arg;
using RelayCommand = GalaSoft.MvvmLight.Command.RelayCommand;

namespace Wisdom.HomeAutomation.WpfApp.ViewModel
{
    public class SchemeViewModel : ViewModelBase
    {
        public SchemeViewModel()
        {
            #region 初始化方案

            using (var db = new HomeAutomationEntities())
            {
                Schemes = db.t_Scheme.ToList();
            }
            if (Schemes.Count > 0)
            {
                Scheme = Schemes[0];
                SchemeName = Schemes[0].SchemeName;
            }

            #endregion

            #region 初始化详细方案//test

            RefreshDetailScheme();

            #endregion

            #region 初始化电器

            using (var db = new HomeAutomationEntities())
            {
                HomeAppliances = db.t_HomeAppliances.ToList();
            }

            #endregion

            #region 新建方案

            CreateSchemeCommand = new RelayCommand(() =>
            {
                if (SchemeState)
                {
                    UMessageBox.Show($"请先停止方案", false);
                    return;
                }
                if (SchemeNameEidtState)
                {
                    
                    return;
                }
                using (var db = new HomeAutomationEntities())
                {
                    var count = db.t_Scheme.ToList().Count;
                    db.t_Scheme.Add(new t_Scheme()
                    {

                        SchemeName = $"方案{++count}"

                    });
                    db.SaveChanges();
                    Schemes = db.t_Scheme.ToList();
                    Scheme = Schemes.Find(x => x.SchemeName == $"方案{count}");
                    DetailSchemes = new ObservableCollection<DetailSchemeModel>();
                }

            });

            #endregion

            #region 删除方案

            DeleteSchemeCommand = new RelayCommand(() =>
            {
                if (SchemeState)
                {
                    UMessageBox.Show($"请先停止方案", false);
                    return;
                }
                if (SchemeNameEidtState)
                {
                    return;
                }
                var result = UMessageBox.Show("确定删除当前方案吗");
                if (result == false)
                {
                    return;
                }
                using (var db = new HomeAutomationEntities())
                {
                    if (Scheme == null)
                    {
                        return;
                    }
                    var scheme = db.t_Scheme.FirstOrDefault(x => x.Id == Scheme.Id);
                    if (scheme != null) db.t_Scheme.Remove(scheme);
                    var detailScheme = db.t_DetailScheme.Where(x => x.SchemeId == Scheme.Id);
                    db.t_DetailScheme.RemoveRange(detailScheme);
                    db.SaveChanges();
                    Schemes = db.t_Scheme.ToList();
                    if (Schemes.Count <= 0) return;
                    Scheme = Schemes[0];
                    SchemeName = Schemes[0].SchemeName;
                }
            });


            #endregion

            #region 刷新方案列表命令

            HomeApplianceScheme = new RelayCommand(() =>
            {
                if (SchemeNameEidtState)
                {
                    return;
                }
                RefreshDetailScheme();

            });

            #endregion

            #region 方案定时开关 （开关定时任务）

            StartSchemeCommand = new RelayCommand((() =>
            {
                if (!UMessageBox.Result)
                {
                    return;
                }
                //是否选择方案，执行方案是否为空
                if (Scheme == null )
                {
                    UMessageBox.Show($"当前没有选中方案", false);
                    return;
                }
                //是否正在编辑方案
                if (SchemeNameEidtState)
                {
                    return;
                }
                //定时方案停止
                if (SchemeState)
                {
                    //
                    //等待提示框

                    //强制关闭所有电器
                    //var driver =new Rs485Driver();
                    //try
                    //{
                    //    driver.Connect(new NetArg("192.168.0.233", 10001));
                    //}
                    //catch (Exception e)
                    //{
                    //    MessageBox.Show($"连接不上设备", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    //    return;
                    //}
                    //var state=  driver.Read32RelayOutputState();
                    //foreach (var value in state)
                    //{
                    //    if (value.Value==true)
                    //    {
                    //        ControllerHomeAutomation(value.Key, false);
                    //    }
                    //}
                    
                    foreach (var value in DetailSchemes)
                    {
                        
                        value.RunSchemeState = false;
                        value.SchemevisibiState = "Hidden";
                        value.Cancel.Cancel();

                    }
                    SchemeState = false;
                    SchemeList = true;
                }
                //定时方案启动
                else
                {
                    SchemeState = true;
                    SchemeList = false;
                    foreach (var value in DetailSchemes)
                    {
                        value.Cancel = new CancellationTokenSource();
                        value.Task = new Task(() =>
                        {
                            //等待开始
                            var waitTime = 0;
                            while (waitTime < value.DtailScheme.WaitTime)
                            {
                                try
                                {
                                    value.Cancel.Token.ThrowIfCancellationRequested();
                                }
                                catch (Exception e)
                                {
                                    value.RunSchemeState = false;
                                    value.SchemevisibiState = "Hidden";
                                    return;
                                }
                                waitTime++;
                                Thread.Sleep(1000);
                            }

                            value.RunSchemeState = true;
                            value.SchemevisibiState = "Visible";
                            //启动电器
                            //try
                            //{
                            //   
                            //    AsyncControllerHomeAutomation(value.DtailScheme.HomeAppliancesId, true);
                            //}
                            //catch (Exception e)
                            //{
                            //    MessageBox.Show($"连接不上设备", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            //    value.RunSchemeState = false;
                            //    value.SchemevisibiState = "Hidden";
                            //    return;
                            //}
                            //执行时长
                            var runTime = 0;
                             
                        //等待结束
                            while (runTime < value.DtailScheme.RunTime)
                            {
                               
                                var timeSpan =new TimeSpan(0,0, value.DtailScheme.RunTime - runTime-1);
                                value.SchemeTime = timeSpan.Hours.ToString()+" h "+timeSpan.Minutes+" m "+timeSpan.Seconds+" s";
                                try
                                {
                                    value.Cancel.Token.ThrowIfCancellationRequested();
                                }
                                catch (Exception e)
                                {
                                    value.RunSchemeState = false;
                                    value.SchemevisibiState = "Hidden";
                                    return;
                                }
                                runTime++;
                                Thread.Sleep(1000);
                            }
                            //关闭电器
                            //try
                            //{
                                
                            //    AsyncControllerHomeAutomation(value.DtailScheme.HomeAppliancesId, false);
                            //}
                            //catch (Exception e)
                            //{
                            //    MessageBox.Show($"连接不上设备", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            //}
                            value.RunSchemeState = false;
                        }, value.Cancel.Token);
                        value.Task.Start();
                    }
                }
            }));

            #endregion

            #region 添加 定时命令

            AddSchemeCommand = new RelayCommand(() =>
            {
                if (DetailSchemes.FirstOrDefault(x => x.DtailScheme.HomeAppliancesName == "选择电器") != null)
                {
                    //提示选择电器
                    UMessageBox.Show($"请选择电器", false);
                    return;
                }
                using (var db = new HomeAutomationEntities())
                {


                    //遍历详细方案
                    for (var i = 0; i < DetailSchemes.Count; i++)
                    {
                        if (DetailSchemes[i].DtailScheme.WaitTime <= 0 || DetailSchemes[i].DtailScheme.RunTime <= 0)
                        {
                            UMessageBox.Show($"{DetailSchemes[i].DtailScheme.HomeAppliancesName} 时间运行太短", false);
                            return;
                        }
                        //id= 100表示没有选中
                        if (DetailSchemes[i] == null || DetailSchemes[i].DtailScheme.HomeAppliancesId == 100)
                        {
                            continue;
                        }
                        //判断时间重复
                        for (var j = 0; j < i; j++)
                        {
                            //根据电器名称相同，断定同一电器
                            if (DetailSchemes[i].DtailScheme.HomeAppliancesName != DetailSchemes[j].DtailScheme.HomeAppliancesName)
                            {
                            }
                            //判断同一电器启动，一分钟间隔以上
                            else if (DetailSchemes[i].DtailScheme.WaitTime >
                                     (DetailSchemes[j].DtailScheme.RunTime +
                                      DetailSchemes[j].DtailScheme.WaitTime) + 60 ||
                                     DetailSchemes[i].DtailScheme.RunTime + DetailSchemes[i].DtailScheme.WaitTime + 60 <
                                     DetailSchemes[j].DtailScheme.WaitTime)
                            {
                            }
                            else
                            {
                                UMessageBox.Show($"{DetailSchemes[i].DtailScheme.HomeAppliancesName} 时间冲突", false);
                                return;
                            }
                        }
                        var detail = DetailSchemes[i];
                        var homeAppliance =
                            db.t_HomeAppliances.FirstOrDefault(
                                x => x.Name == detail.DtailScheme.HomeAppliancesName);
                        if (homeAppliance != null)
                            db.t_DetailScheme.AddOrUpdate(new t_DetailScheme()
                            {
                                Id = DetailSchemes[i].DtailScheme.DetailSchemeId,
                                HomeAppliancesId = homeAppliance.Id,
                                RunTime = DetailSchemes[i].DtailScheme.RunTime,
                                WaitTime = DetailSchemes[i].DtailScheme.WaitTime,
                                SchemeId = DetailSchemes[i].DtailScheme.SchemeId
                            });
                    }
                    db.SaveChanges();
                    Schemes = db.t_Scheme.ToList();
                    Scheme = Schemes.FirstOrDefault(x => x.Id == SchemmeId);
                }
                RefreshDetailScheme();
                DetailSchemes.Add(new DetailSchemeModel()
                {
                    DtailScheme = new v_DtailScheme()
                    {
                        RunTime = 0,
                        WaitTime = 0,
                        SchemeId = Scheme.Id,
                        HomeAppliancesName = "选择电器"
                    }
                });
            });

            #endregion

            #region 更新选中定时

            UpdataSchemeCommand = new RelayCommand<long>((detailSchemeId) =>
            {
                var detailscheme = DetailSchemes.FirstOrDefault(x => x.DtailScheme.DetailSchemeId == detailSchemeId);
                //id 100表示没有选中
                if (detailscheme == null || detailSchemeId == 100)
                {
                    return;
                }
                //判断时间重复

                using (var db = new HomeAutomationEntities())
                {
                    var homeAppliance =
                        db.t_HomeAppliances.FirstOrDefault(x => x.Name == detailscheme.DtailScheme.HomeAppliancesName);
                    var detailScheme =
                        db.t_DetailScheme.FirstOrDefault(x => x.Id == detailscheme.DtailScheme.DetailSchemeId);
                    if (detailScheme == null || homeAppliance == null)
                    {
                        return;
                    }
                    detailScheme.HomeAppliancesId = homeAppliance.Id;
                    detailScheme.RunTime = detailscheme.DtailScheme.RunTime;
                    detailScheme.WaitTime = detailscheme.DtailScheme.WaitTime;

                    db.SaveChanges();
                }
                RefreshDetailScheme();
            });

            #endregion

            #region 删除选中定时命令

            DeleletChooseDetailScheme = new RelayCommand<long>(((detailSchemeId) =>
            {
                using (var db = new HomeAutomationEntities())
                {
                    //删除定时
                    var descheme = db.t_DetailScheme.FirstOrDefault(x => x.Id == detailSchemeId);
                    if (descheme != null) db.t_DetailScheme.Remove(descheme);
                    db.SaveChanges();
                }
                DetailSchemes.Remove(DetailSchemes.FirstOrDefault(x => x.DtailScheme.DetailSchemeId == detailSchemeId));
            }));

            #endregion

            #region 编辑选中方案

            UpdataChooseDatailScheme = new RelayCommand((() =>
            {
                if (SchemeState)
                {
                    UMessageBox.Show($"请先停止方案", false);
                    return;
                }
                if (SchemeName == null)
                {
                    return;
                }

                if (SchemeEidtState)
                {
                    SchemmeId = Scheme.Id;
                    SchemeEidtState = false;
                    ButtonNameSave = "完成";
                    SchemeAddState = "Visible";
                    SchemeNameEidtState = true;
                }
                else
                {
                    for (var i = DetailSchemes.Count() - 1; i >= 0; i--)
                    {
                        if (DetailSchemes[i].DtailScheme.HomeAppliancesName == "选择电器")
                        {
                            DetailSchemes.Remove(DetailSchemes[i]);
                        }
                    }
                    using (var db = new HomeAutomationEntities())
                    {

                        //更新方案名称
                        var scheme = db.t_Scheme.FirstOrDefault(x => x.Id == SchemmeId);
                        if (scheme != null) scheme.SchemeName = SchemeName;



                        //遍历详细方案
                        for (var i = 0; i < DetailSchemes.Count; i++)
                        {
                            if (DetailSchemes[i].DtailScheme.WaitTime<=0|| DetailSchemes[i].DtailScheme.RunTime <=0)
                            {
                                UMessageBox.Show($"{DetailSchemes[i].DtailScheme.HomeAppliancesName} 时间运行太短",false);
                                return;
                            }
                            //id= 100表示没有选中
                            if (DetailSchemes[i] == null || DetailSchemes[i].DtailScheme.HomeAppliancesId == 100)
                            {
                                continue;
                            }
                            //判断时间重复
                            for (var j = 0; j < i; j++)
                            {
                                //根据电器名称相同，断定同一电器
                                if (DetailSchemes[i].DtailScheme.HomeAppliancesName != DetailSchemes[j].DtailScheme.HomeAppliancesName)
                                {
                                }
                                //判断同一电器启动，一分钟间隔以上
                                else if (DetailSchemes[i].DtailScheme.WaitTime >
                                         (DetailSchemes[j].DtailScheme.RunTime +
                                          DetailSchemes[j].DtailScheme.WaitTime) + 60 ||
                                         DetailSchemes[i].DtailScheme.RunTime + DetailSchemes[i].DtailScheme.WaitTime + 60 <
                                         DetailSchemes[j].DtailScheme.WaitTime)
                                {
                                }
                                else
                                {
                                    UMessageBox.Show($"{DetailSchemes[i].DtailScheme.HomeAppliancesName} 时间冲突", false);
                                    return;
                                }
                            }
                            var detail = DetailSchemes[i];
                            var homeAppliance =
                                db.t_HomeAppliances.FirstOrDefault(
                                    x => x.Name == detail.DtailScheme.HomeAppliancesName);
                            if (homeAppliance != null)
                                db.t_DetailScheme.AddOrUpdate(new t_DetailScheme()
                                {
                                    Id = DetailSchemes[i].DtailScheme.DetailSchemeId,
                                    HomeAppliancesId = homeAppliance.Id,
                                    RunTime = DetailSchemes[i].DtailScheme.RunTime,
                                    WaitTime = DetailSchemes[i].DtailScheme.WaitTime,
                                    SchemeId = DetailSchemes[i].DtailScheme.SchemeId
                                });
                        }
                        db.SaveChanges();
                        Schemes = db.t_Scheme.ToList();
                        Scheme = Schemes.FirstOrDefault(x => x.Id == SchemmeId);
                    }

                    RefreshDetailScheme();
                    SchemeEidtState = true;
                    ButtonNameSave = "方案编辑";
                    SchemeAddState = "Hidden";
                    SchemeNameEidtState = false;
                }
            }));

            #endregion
        }

        private string _schemeBtnName = "开始";

        /// <summary>
        /// 方案开始按钮
        /// </summary>
        public string SchemeBtnName
        {
            get { return _schemeBtnName; }
            set { Set(() => SchemeBtnName, ref _schemeBtnName, value); }
        }

        private string _buttonNameSave = "方案编辑";

        /// <summary>
        /// 方案编辑按钮
        /// </summary>
        public string ButtonNameSave
        {
            get { return _buttonNameSave; }
            set { Set(() => ButtonNameSave, ref _buttonNameSave, value); }
        }

        private List<t_Scheme> _schemes;

        /// <summary>
        /// 总方案列表
        /// </summary>
        public List<t_Scheme> Schemes
        {
            get { return _schemes; }
            set { Set(() => Schemes, ref _schemes, value); }
        }

        private t_Scheme _scheme;

        private bool _schemeList = true;
        /// <summary>
        /// 是否可以切换方案
        /// </summary>
        public bool SchemeList
        {
            get { return _schemeList;}
            set { Set(()=>SchemeList,ref _schemeList,value); }
        }

        /// <summary>
        /// 当前选择方案
        /// </summary>
        public t_Scheme Scheme
        {
            get { return _scheme; }
            set { Set(() => Scheme, ref _scheme, value); }
        }

        public long SchemmeId { set; get; }

        private string _schemeName;
        /// <summary>
        /// 重命名方案名称
        /// </summary>
        public string SchemeName
        {
            get { return _schemeName; }
            set { Set(() => SchemeName, ref _schemeName, value); }
        }

        private List<t_CommandType> _commandType;

        /// <summary>
        /// 定时命令列表
        /// </summary>
        public List<t_CommandType> CommandType
        {
            get { return _commandType; }
            set { Set(() => CommandType, ref _commandType, value); }
        }

        private List<t_HomeAppliances> _homeAppliances;

        /// <summary>
        /// 所有家电选项
        /// </summary>
        public List<t_HomeAppliances> HomeAppliances
        {
            get { return _homeAppliances; }
            set { Set(() => HomeAppliances, ref _homeAppliances, value); }
        }

        private bool _schemeEidtState = true;

        /// <summary>
        /// 表格是否编辑
        /// </summary>
        public bool SchemeEidtState
        {
            get { return _schemeEidtState; }
            set { Set(() => SchemeEidtState, ref _schemeEidtState, value); }
        }

        private bool _schemeNameEidtState = false;

        /// <summary>
        /// 方案名称是否编辑
        /// </summary>
        public bool SchemeNameEidtState
        {
            get { return _schemeNameEidtState; }
            set { Set(() => SchemeNameEidtState, ref _schemeNameEidtState, value); }
        }


        private bool _schemeState = false;
        /// <summary>
        /// 方案是否启动
        /// </summary>
        public bool SchemeState
        {
            get { return _schemeState; }
            set { Set(() => SchemeState, ref _schemeState, value); }
        }

        private string _schemeRemainTime = "Hidden";
        /// <summary>
        /// 是否显示剩余时长
        /// </summary>
        public string SchemeRemainTime
        {
            get { return _schemeRemainTime; }
            set { Set(() => SchemeRemainTime, ref _schemeRemainTime, value); }
        }

        private string _schemeAddState = "Hidden";
        /// <summary>
        /// 添加按钮显示
        /// </summary>
        public string SchemeAddState
        {
            get { return _schemeAddState; }
            set { Set(() => SchemeAddState, ref _schemeAddState, value); }
        }

        /// <summary>
        /// 详细方案列表
        /// </summary>
        private ObservableCollection<DetailSchemeModel> _detailSchemes = new ObservableCollection<DetailSchemeModel>();

        /// <summary>
        /// 方案列表
        /// </summary>
        public ObservableCollection<DetailSchemeModel> DetailSchemes
        {
            get { return _detailSchemes; }
            set { Set(() => DetailSchemes, ref _detailSchemes, value); }
        }

        private t_HomeAppliances _chooseSchemeHomeAppliance = null;

        public t_HomeAppliances ChooseSchemeHomeAppliance
        {
            get { return _chooseSchemeHomeAppliance; }
            set { Set(() => ChooseSchemeHomeAppliance, ref _chooseSchemeHomeAppliance, value); }
        }

        public ICommand CreateSchemeCommand { get; }

        public ICommand DeleteSchemeCommand { get; }
        /// <summary>
        /// 定时方案开关
        /// </summary>
        public ICommand StartSchemeCommand { get; }

        public ICommand HomeApplianceScheme { get; }

        /// <summary>
        /// 添加电器定时命令
        /// </summary>
        public ICommand AddSchemeCommand { get; }

        /// <summary>
        /// 更新选中定时
        /// </summary>
        public ICommand UpdataSchemeCommand { get; }

        /// <summary>
        /// 删除选中定时命令
        /// </summary>
        public ICommand DeleletChooseDetailScheme { get; }

        /// <summary>
        /// 编辑方案
        /// </summary>
        public ICommand UpdataChooseDatailScheme { get; }

        /// <summary>
        /// 刷新当前方案列表
        /// </summary>
        public void RefreshDetailScheme()
        {
            DetailSchemes = new ObservableCollection<DetailSchemeModel>();
            using (var db = new HomeAutomationEntities())
            {
                if (Scheme == null) return;
                var detailSchemes = db.v_DtailScheme.Where(x => x.SchemeId == Scheme.Id).ToList();
                foreach (var value in detailSchemes)
                {
                    DetailSchemes.Add(new DetailSchemeModel()
                    {
                        DtailScheme = value
                    });
                }
            }
        }
        /// <summary>
        /// 异步设置电器开关
        /// </summary>
        /// <param name="address"></param>
        /// <param name="state"></param>
        public async void AsyncControllerHomeAutomation(int address,bool state)
        {
            var driver = new Rs485Driver();
            try
            {
                driver.Connect(new NetArg("192.168.0.233", 10001));
            }
            catch (Exception e)
            {
                MessageBox.Show($"连接不上设备", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            switch (address)
            {
                case 0:
                    if (state)
                    {
                        driver.InstallRelayOutputState(0, true);
                        await Task.Delay(10000);
                        driver.InstallRelayOutputState(13, true);
                    }
                    else
                    {
                        driver.InstallRelayOutputState(13, false);
                        await Task.Delay(10000);
                        driver.InstallRelayOutputState(0, false);
                    }
                    break;
                case 6:
                    if (state)
                    {
                        driver.InstallRelayOutputState(6, true);
                        await Task.Delay(10000);
                        driver.InstallRelayOutputState(18, true);
                    }
                    else
                    {
                        driver.InstallRelayOutputState(18, false);
                        await Task.Delay(10000);
                        driver.InstallRelayOutputState(6, false);
                    }
                    break;
                case 10:
                    if (state)
                    {
                        driver.InstallRelayOutputState(10, true);
                        await Task.Delay(10000);
                        driver.InstallRelayOutputState(17, true);
                    }
                    else
                    {
                        driver.InstallRelayOutputState(17, false);
                        await Task.Delay(10000);
                        driver.InstallRelayOutputState(10, false);
                    }
                    break;
                case 5:
                    if (state)
                    {
                        driver.InstallRelayOutputState(5, true);
                        await Task.Delay(8000);
                        driver.InstallRelayOutputState(15, true);
                        await Task.Delay(500);
                        driver.InstallRelayOutputState(15, false);
                        await Task.Delay(1000);
                        driver.InstallRelayOutputState(14, true);
                    }
                    else
                    {
                        driver.InstallRelayOutputState(14, false);
                        driver.InstallRelayOutputState(5, false);
                    }
                    break;
                default: driver.InstallRelayOutputState(address, state); break;
            }
        }
        /// <summary>
        /// 同步设置电器开关
        /// </summary>
        /// <param name="address"></param>
        /// <param name="state"></param>
        public  void ControllerHomeAutomation(int address, bool state)
        {
            var driver = new Rs485Driver();
            try
            {
                driver.Connect(new NetArg("192.168.0.233", 10001));
            }
            catch (Exception e)
            {
                MessageBox.Show($"连接不上设备", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            switch (address)
            {
                case 0:
                    if (state)
                    {
                        driver.InstallRelayOutputState(0, true);
                        Thread.Sleep(10000);
                        driver.InstallRelayOutputState(13, true);
                    }
                    else
                    {
                        driver.InstallRelayOutputState(13, false);
                        Thread.Sleep(10000);
                        driver.InstallRelayOutputState(0, false);
                    }
                    break;
                case 6:
                    if (state)
                    {
                        driver.InstallRelayOutputState(6, true);
                        Thread.Sleep(10000);
                        driver.InstallRelayOutputState(18, true);
                    }
                    else
                    {
                        driver.InstallRelayOutputState(18, false);
                        Thread.Sleep(10000);
                        driver.InstallRelayOutputState(6, false);
                    }
                    break;
                case 10:
                    if (state)
                    {
                        driver.InstallRelayOutputState(10, true);
                        Thread.Sleep(10000);
                        driver.InstallRelayOutputState(17, true);
                    }
                    else
                    {
                        driver.InstallRelayOutputState(17, false);
                        Thread.Sleep(10000);
                        driver.InstallRelayOutputState(10, false);
                    }
                    break;
                case 5:
                    if (state)
                    {
                        driver.InstallRelayOutputState(5, true);
                        Thread.Sleep(8000);
                        driver.InstallRelayOutputState(15, true);
                        Thread.Sleep(500);
                        driver.InstallRelayOutputState(15, false);
                        Thread.Sleep(1500);
                        driver.InstallRelayOutputState(14, true);
                    }
                    else
                    {
                        driver.InstallRelayOutputState(14, false);
                        driver.InstallRelayOutputState(5, false);
                    }
                    break;
                default: driver.InstallRelayOutputState(address, state); break;
            }
        }

    }
}