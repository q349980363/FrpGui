using IniFile;
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

        //private IniFile ini = new IniFile("FrpGui.ini");


        /// <summary>
        /// 所有Key
        /// </summary>
        public BindingList<Property> KeyList { get; set; } = new BindingList<Property>();

        /// <summary>
        /// 所有Section
        /// </summary>
        public BindingList<IniFile.Section> TemplateList { get; set; } = new BindingList<IniFile.Section>();



        Ini ini;
        public EditorWindow() {
            //var list = MainWindow.ini.Select(v => v.Name);
            MainWindow.ini.ToList().ForEach(v => {
                TemplateList.Add(v);
            });
        }

        //编辑最好传递个新的过去,别传递旧的过去
        //用于编辑
        public EditorWindow(Ini ini, string Name) : this() {
            this.ini = ini;
            this.NewName = Name;
            this.OldName = Name;
            this.DataContext = this;
            init();
            InitializeComponent();
        }

        ////用于创建的不能初始化数据
        //public EditorWindow(Ini data, string NewName) : this() {
        //    this.data = data;
        //    this.OldName = "无";
        //    this.NewName = NewName;
        //    this.DataContext = this;
        //    InitializeComponent();
        //}

        void init() {
            this.KeyList.Clear();

            if (ini[OldName] == null) {
                return;
            }
            var keyList = ini[OldName];
            foreach (var key in keyList) {
                this.KeyList.Add(key);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IniItemControl_Click(object sender, RoutedEventArgs e) {
            var item = sender as IniItemControl;
            var data = item.DataContext as Property;
            if (!string.IsNullOrEmpty(OldName))
                ini[OldName].Remove(data);
            KeyList.Remove(data);
        }

        /// <summary>
        /// 新增属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e) {
            if (KeyList.Any(v => v.Name == Key)) {
                return;
            }
            var p = new Property(Key, Value);
            if (!string.IsNullOrEmpty(OldName))
                ini[OldName].Add(p);
            KeyList.Add(p);
        }

        /// <summary>
        /// 清空所有
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e) {
            KeyList.Clear();
            if (!string.IsNullOrEmpty(OldName))
                ini[OldName].Clear();

        }

        /// <summary>
        /// 导入某一项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e) {
            var item = sender as Button;
            var data = item.DataContext as IniFile.Section;
            //var keyList = ini.ReadList(null, data.Section);



            //KeyList.Clear();
            foreach (var key in data) {
                //key.Items
                var p = new Property(key.Name, key.Value, key.Items.Select(v => v.ToString()).ToArray());
                if (!string.IsNullOrEmpty(OldName))

                    ini[OldName].Add(p);

                KeyList.Add(p);
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_4(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(NewName)) {
                MessageBox.Show("请输入项名称!", "提示", MessageBoxButton.OK);
                return;
            }
            if (KeyList.Count == 0) {
                MessageBox.Show("导入数据空!", "提示", MessageBoxButton.OK);
                return;
            }
            this.DialogResult = true;
        }
    }
}
