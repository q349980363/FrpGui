using IniFile;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FrpGui {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {


        protected override void OnStartup(StartupEventArgs e) {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            IniLoadSettings.Default.DetectEncoding = false;
            //Encoding GB2132 = Encoding.GetEncoding("GB2312");
            IniLoadSettings.Default.Encoding = Encoding.UTF8;
            base.OnStartup(e);
        }
    }
}
