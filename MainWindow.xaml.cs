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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {
        public string[] EXEList { get; set; }
        public string[] ConfigList { get; set; }
        public BindingList<IniData> SectionList { get; set; } = new BindingList<IniData>();
        public IniData Common { get; set; }
        public MainWindow() {
            InitFile();
            DataContext = this;
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        void LoadConfig(string path) {
            var ini = new IniFile(path);
            var sectionList = ini.ReadList(null, null);
            sectionList.Remove("common");
            Common = new IniData(ini, "common");
            this.SectionList.Clear();
            sectionList.Select(v => new IniData(ini, v)).ToList().ForEach(v => this.SectionList.Add(v));
        }

        void InitFile() {
            EXEList = System.IO.Directory.GetFiles(".\\", "*.exe");
            ConfigList = System.IO.Directory.GetFiles(".\\", "*.ini");
        }

        private void ComboBox_config_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var item = sender as ComboBox;
            var path = item.SelectedItem as string;
            path = System.IO.Path.GetFullPath(path);
            LoadConfig(path);
        }

        private void ComboBox_exe_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }
    }
}
