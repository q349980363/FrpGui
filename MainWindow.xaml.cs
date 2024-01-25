using IniFile;
using Octokit;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace FrpGui {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {
        static MainWindow() {

        }
        public MyCommand[] EXEList { get; set; }
        public MyCommand[] ConfigList { get; set; }
        public BindingList<IniFile.Section> SectionList { get; set; } = new BindingList<IniFile.Section>();

        /// <summary>
        /// 配置文件
        /// </summary>
        static public Ini ini = new Ini("FrpGui.ini");



        public string path_exe { get; set; }
        public string path_config { get; set; }

        public MainWindow() {
            InitFile();
            DataContext = this;
            InitializeComponent();
            Task.Factory.StartNew(async () => {
                try {
                    var client = new GitHubClient(new ProductHeaderValue("my-cool-app"));
                    var releases = await client.Repository.Release.GetAll("fatedier", "frp");
                    var latest = releases[0];
                    this.Dispatcher.Invoke(() => {
                        run_frpversion.Text = "frp版本:" + latest.Name;
                    });
                } catch (Exception e) {

                    throw;
                }

            });
        }

        public ICommand GoCommand { get; private set; }

        private void OnGoCommand(object obj) {
        }

        private bool CanGoCommand(object obj) {
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void LoadConfig(string path) {
            try {
                if (string.IsNullOrEmpty(path)) {
                    return;
                }
                var ini = new Ini(path);
                //var b = ini["a"];
                var sectionList = ini.OrderBy(v => v.Name);
                this.SectionList.Clear();
                sectionList.ToList().ForEach(v => {
                    this.SectionList.Add(v);
                });
            } catch (Exception) {
                MessageBox.Show("加载失败!", "提示", MessageBoxButton.OK);
                return;
            }
        }

        private void InitFile() {
            path_exe = ini["app"]["path_exe"];
            path_config = ini["app"]["path_config"];
            EXEList = System.IO.Directory.GetFiles(".\\", "*.exe").Select(v => new MyCommand { Text = v }).ToArray();
            ConfigList = System.IO.Directory.GetFiles(".\\", "*.ini").Select(v => new MyCommand { Text = v }).ToArray();
            this.EXEList.ToList().ForEach(item => {
                item.Checked = item.Text == path_exe;
            });
            this.ConfigList.ToList().ForEach(item => {
                item.Checked = item.Text == path_config;
            });

            LoadConfig(path_config);
        }

        private void ComboBox_config_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            //if (!init)
            //    return;
            var item = sender as ComboBox;
            var path = item.SelectedItem as string;
            path = System.IO.Path.GetFullPath(path);
            LoadConfig(path);
        }

        private void ComboBox_exe_SelectionChanged(object sender, SelectionChangedEventArgs e) {
        }

        private void Window_Closing(object sender, CancelEventArgs e) {
            if (exeProcess != null)
                exeProcess.Kill();
            Properties.Settings.Default.Save();
        }

        private Process exeProcess;

        private void start() {
            menuItem_start.IsEnabled = false;
            menuItem_restart.IsEnabled = menuItem_stop.IsEnabled = true;
            //var path = comboBox_exe.SelectedItem as string;
            //var path_config = comboBox_config.SelectedItem as string;
            menuItem_stop.IsChecked = !false;
            menuItem_start.IsChecked = !true;

            ProcessStartInfo info = new ProcessStartInfo(path_exe, " -c " + path_config);//控制台exe信息
            info.UseShellExecute = false;
            info.RedirectStandardInput = true;//输入流重定向为true
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.CreateNoWindow = true;

            info.WindowStyle = ProcessWindowStyle.Hidden;//隐藏窗口

            exeProcess = new Process();
            exeProcess.StartInfo = info;
            exeProcess.ErrorDataReceived += ExeProcess_ErrorDataReceived;
            exeProcess.OutputDataReceived += ExeProcess_OutputDataReceived;
            exeProcess.EnableRaisingEvents = true;
            exeProcess.Exited += Process_Exited;
            exeProcess.Start();
            exeProcess.BeginErrorReadLine();
            exeProcess.BeginOutputReadLine();

            Properties.Settings.Default.autoRun = true;
        }

        private void ExeProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
            appendLog(e.Data);
        }

        private void ExeProcess_OutputDataReceived(object sender, DataReceivedEventArgs e) {
            appendLog(e.Data);
        }

        private void appendLog(string txt) {
            this.Dispatcher.Invoke(() => {
                if (textBox_log.Text.Length > 10000) {
                    textBox_log.Text = "";
                }
                textBox_log.AppendText(txt + "\r\n");
            });
        }

        private void Process_Exited(object? sender, EventArgs e) {
            Properties.Settings.Default.autoRun = false;
            this.Dispatcher.Invoke(() => {
                menuItem_start.IsEnabled = true;
                menuItem_stop.IsEnabled = menuItem_restart.IsEnabled = false;
                menuItem_stop.IsChecked = !true;
                menuItem_start.IsChecked = !false;
            });
            exeProcess = null;
        }

        private void Window_Initialized(object sender, EventArgs e) {
            //var list = Process.GetProcessesByName("frpc.exe");
            //if (MessageBox.Show("Delete this user?", "Confirm Message", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK) {
            //}
        }

        private void menuItem_start_Click(object sender, RoutedEventArgs e) {
            start();
        }

        private void menuItem_stop_Click(object sender, RoutedEventArgs e) {
            exeProcess.Kill();
        }

        private void menuItem_restart_Click(object sender, RoutedEventArgs e) {
            exeProcess.Kill();
            var t = DateTime.Now.AddMilliseconds(800);
            while (DateTime.Now < t)
                DispatcherHelper.DoEvents();
            start();
        }

        public class MyCommand : INotifyPropertyChanged {
            public ICommand Command { get; set; }
            public string Text { get; set; }
            public string Parameter { get; set; }
            public Boolean Checked { get; set; }

            public event PropertyChangedEventHandler? PropertyChanged;
        }

        private void MenuItem_EXE_Click(object sender, RoutedEventArgs e) {
            var item = sender as MenuItem;
            var data = item.DataContext as MyCommand;
            this.path_exe = data.Text;
            //ini.Write("path_exe", data.Text, "app");
            ini["app"]["path_exe"] = data.Text;
            ini.SaveTo("FrpGui.ini");
            this.EXEList.ToList().ForEach(item => {
                item.Checked = item == data;
            });
        }
        private void MenuItem_Config_Click(object sender, RoutedEventArgs e) {
            var item = sender as MenuItem;
            var data = item.DataContext as MyCommand;
            this.path_config = data.Text;
            ini["app"]["path_config"] = data.Text;
            ini.SaveTo("FrpGui.ini");
            this.ConfigList.ToList().ForEach(item => {
                item.Checked = item == data;
            });
            LoadConfig(path_config);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IniSectionControl_ClickDel(object sender, RoutedEventArgs e) {
            var item = sender as IniSectionControl;
            var data = item.DataContext as IniFile.Section;
            this.SectionList.Remove(data);
            //用旧版本删除也无所谓
            _IniFile.Write(null, null, data.Name, path_config);
            LoadConfig(path_config);
        }

        /// <summary>
        /// 点击编辑  点击前点击后?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IniSectionControl_ClickEditor(object sender, RoutedEventArgs e) {
            var item = sender as IniSectionControl;
            var data = item.DataContext as IniFile.Section;



            var inidata = new Ini(path_config);
            var win = new EditorWindow(inidata, data.Name);
            win.Owner = Window.GetWindow(this);
            var ret = win.ShowDialog();
            if (win.DialogResult != true) {
                return;
            }

            if (win.NewName != win.OldName) {
                //也就是说改名了
                inidata[win.OldName].Name = win.NewName;
            }

            if (inidata[win.NewName] == null) {
                inidata.Add(new Section(win.NewName));
            }
            if (ret.Value) {

                inidata.SaveTo(path_config);

                //IniData.File.Write(null, null, win.OldName);
                //主界面处理编辑逻辑
                LoadConfig(path_config);
            }

        }

        /// <summary>
        /// 新增配置项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem_add_Click(object sender, RoutedEventArgs e) {
            var inidata = new Ini(path_config);
            var win = new EditorWindow(inidata, "");
            win.Owner = Window.GetWindow(this);
            win.ShowDialog();
            if (win.DialogResult != true || string.IsNullOrEmpty(win.NewName)) {
                return;
            }
            if (string.IsNullOrEmpty(win.NewName)) {
                return;
            }

            inidata = new Ini(path_config);

            if (inidata[win.NewName] == null) {
                inidata.Add(new Section(win.NewName));
            }
            //TODO 新增写出等下写.
            win.KeyList.ToList().ForEach(v => {
                inidata[win.NewName].Add(v);
                //inidata.File.Write(, v.Value, win.NewName);
            });

            inidata.SaveTo(path_config);
            LoadConfig(path_config);
        }
    }

    public static class DispatcherHelper {

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void DoEvents() {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrames), frame);
            try { Dispatcher.PushFrame(frame); } catch (InvalidOperationException) { }
        }

        private static object ExitFrames(object frame) {
            ((DispatcherFrame)frame).Continue = false;
            return null;
        }
    }
}
