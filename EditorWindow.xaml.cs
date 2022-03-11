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
using System.Windows.Shapes;

namespace FrpGui {
    /// <summary>
    /// EditorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditorWindow : Window {
        public string Key { get; set; }
        public string Value { get; set; }

        public string OldName { get; set; }
        public string NewName { get; set; }

        private IniFile ini = new IniFile("FrpGui.ini");
        public BindingList<IniData> KeyList { get; set; } = new BindingList<IniData>();
        public BindingList<IniData> TemplateList { get; set; } = new BindingList<IniData>();


        IniData data;
        public EditorWindow() {
            var list = ini.ReadList(null, null);
            list.ToList().ForEach(v => {
                TemplateList.Add(new IniData(ini, v));
            });
        }

        //用于编辑
        public EditorWindow(IniData data) : this() {
            this.data = data;
            this.NewName = this.OldName = data.Section;
            this.DataContext = this;
            init();
            InitializeComponent();
        }

        //用于创建的不能初始化数据
        public EditorWindow(IniData data, string NewName) : this() {
            this.data = data;
            this.OldName = "无";
            this.NewName = NewName;
            this.DataContext = this;
            InitializeComponent();
        }

        void init() {
            var keyList = data.File.ReadList(null, OldName);
            KeyList.Clear();
            foreach (var key in keyList) {
                KeyList.Add(new IniData(data, key));
            }
        }

        private void IniItemControl_Click(object sender, RoutedEventArgs e) {
            var item = sender as IniItemControl;
            var data = item.DataContext as IniData;
            KeyList.Remove(data);
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            if (KeyList.Any(v => v.Key == Key)) {
                return;
            }
            KeyList.Add(new IniData() { Key = Key, Value = Value });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            KeyList.Clear();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) {
            var item = sender as Button;
            var data = item.DataContext as IniData;
            var keyList = ini.ReadList(null, data.Section);
            //KeyList.Clear();
            foreach (var key in keyList) {
                KeyList.Add(new IniData() { Key = key, Value = ini.Read(key, data.Section), Section = data.Section });
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
        }
    }
}
