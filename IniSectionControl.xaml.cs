using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            this.DataContextChanged += IniSectionControl_DataContextChanged;
            itemsControl.ItemsSource = KeyList;
        }

        public BindingList<IniData> KeyList { get; set; } = new BindingList<IniData>();
        private void IniSectionControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
            groupBox.Header = Header;
            if (Data == null) {
                return;
            }
            var keyList = Data.File.ReadList(null, Section);
            KeyList.Clear();
            foreach (var key in keyList) {
                KeyList.Add(new IniData(Data, key));
            }
        }

        public string Section {
            get {
                return Data == null ? "" : Data.Section;
            }
        }
        public IniData? Data {
            get {
                return this.DataContext as IniData;
            }
        }
        public string Header {
            get {
                if (translation(Section) == Section) {
                    return Section;
                } else {
                    return $"{Section} [{translation(Section)}]";
                }
            }
        }

        string translation(string txt) {
            switch (txt) {
                case "common":
                    return "公共设置";
                default:
                    return txt;
            }
        }

        private void ListView_MouseDown(object sender, MouseButtonEventArgs e) {

        }
    }
}
