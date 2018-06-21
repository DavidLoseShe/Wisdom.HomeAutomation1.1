using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Wisdom.HomeAutomation.Dal;
using Wisdom.Utils;

namespace Wisdom.HomeAutomation.WpfApp.ViewModel
{
   public class DetailSchemeModel: ViewModelBase
   {
        /// <summary>
        /// 定时
        /// </summary>
        public  Task Task{ set; get; }
        public CancellationTokenSource Cancel { get; set; }

        private bool _isEdit = true;

       public bool IsEdit
        {
           get { return _isEdit; }
           set { Set(() => IsEdit, ref _isEdit, value); }
        }

       private string _schemeTime ;
       public string SchemeTime
       {
           get { return _schemeTime; }
           set { Set(() => SchemeTime, ref _schemeTime, value); }
       }

        private string _schemevisibiState = "Hidden";
       public string SchemevisibiState
        {
           get { return _schemevisibiState; }
           set { Set(() => SchemevisibiState, ref _schemevisibiState, value); }
       }

       private bool _runSchemeState;
       public bool RunSchemeState
        {
           get { return _runSchemeState; }
           set { Set(() => RunSchemeState, ref _runSchemeState, value); }
       }

        private string _commandName= "启动";
       public string CommandName
       {
           get { return _commandName; }
           set { Set(()=> DtailScheme == null ? "启动" : $"启动{DtailScheme.HomeAppliancesName}", ref _commandName,value); }
       }

       private v_DtailScheme _deDtailScheme;

       public v_DtailScheme DtailScheme
       {
           get { return _deDtailScheme; }
           set { Set(() => DtailScheme, ref _deDtailScheme, value); }
        }
   }
}
