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
    /// IniSectionControl.xaml 的交互逻辑
    /// </summary>
    public partial class IniSectionControl : UserControl {
        public IniSectionControl() {
            InitializeComponent();
            groupBox.Header = translation(Section);
        }


        public string Section {
            get {
                if (this.DataContext == null) {
                    return "";
                } else {
                    return this.DataContext as string;
                }
            }
        }
        public string Header {
            get {
                return $"{translation(Section)} [{Section}]";
            }
        }

        string translation(string txt) {
            switch (txt) {
                case "公共设置":
                default:
                    return txt;
            }
        }






    }
}
