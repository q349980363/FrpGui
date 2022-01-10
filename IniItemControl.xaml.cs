using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FrpGui {
    /// <summary>
    /// IniItemControl.xaml 的交互逻辑
    /// </summary>
    public partial class IniItemControl : UserControl {

        public IniItemControl() {
            InitializeComponent();
            this.DataContextChanged += IniItemControl_DataContextChanged;
        }

        private void IniItemControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {

            if (Data == null) {
                return;
            }
            label.Content = Header;

        }

        public string Header {
            get {
                if (translation(Data.Key) == Data.Key) {
                    return Data.Key;
                } else {
                    return $"{translation(Data.Key)} \"{Data.Key}\"";
                }
            }
        }

        string translation(string txt) {
            switch (txt) {
                case "type":
                    return "类型[tcp|udp|http|https]";
                case "server_addr":
                    return "服务器地址";
                case "server_port":
                    return "服务器端口";
                default:
                    return txt;
            }
        }
        public IniData? Data {
            get {
                return this.DataContext as IniData;
            }
        }
    }
}
