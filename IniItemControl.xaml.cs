using System;
using System.Windows;
using System.Windows.Controls;

namespace FrpGui {

    /// <summary>
    /// IniItemControl.xaml 的交互逻辑
    /// </summary>
    public partial class IniItemControl : UserControl {

        public IniItemControl() {
            InitializeComponent();
            //grid.DataContext = this;
            dockPanel.DataContext = this;
        }


        public IniData IniData {
            get {
                return (IniData)GetValue(IniDataProperty);
            }
            set {
                SetValue(IniDataProperty, value);
                //loadData();
            }
        }

        public static readonly DependencyProperty IniDataProperty = DependencyProperty.Register("IniData", typeof(IniData), typeof(IniItemControl), new PropertyMetadata(PropertyChangedCallback));

        static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            //d.SetValue(IniDataProperty, e.NewValue);
            var obj = (IniItemControl)d;
            //obj.loadData();
        }

        public bool IsEditor { get; set; }

        public static readonly DependencyProperty IsEditorProperty = DependencyProperty.Register("IsEditor", typeof(bool), typeof(IniItemControl), new PropertyMetadata(null));




        public string Header {
            get {
                if (translation(IniData.Key) == IniData.Key) {
                    return "";
                } else {
                    return $"\"{IniData.Key}\" {translation(IniData.Key)}";
                }
            }
        }

        public static string translation(string txt) {
            switch (txt) {
                case "type":
                    return "类型 代理类型\r\n[tcp, udp, http, https, stcp, sudp, xtcp, tcpmux]";

                case "server_addr":
                    return "服务器地址";

                case "server_port":
                    return "服务器端口";

                case "token":
                    return "密钥";

                case "local_ip":
                    return "本地服务 IP \r\n默认值:127.0.0.1";

                case "local_port":
                    return "本地服务端口 \r\n配合 local_ip";

                case "remote_port":
                    return "服务端绑定的端口\r\n用户访问此端口的请求会被转发到 local_ip:local_port";

                case "custom_domains":
                    return "服务器绑定自定义域名\r\n是(和 subdomain 两者必须配置一个)";
                case "subdomain":
                    return "自定义子域名\r\n是(和 custom_domains 两者必须配置一个)";

                default:
                    return txt;
            }
        }
        public static readonly RoutedEvent ClickEvent =
            EventManager.RegisterRoutedEvent("ClickEvent", RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventArgs<Object>), typeof(IniItemControl));
     
        public event RoutedEventHandler Click {
            //将路由事件添加路由事件处理程序
            add { AddHandler(ClickEvent, value); }
            //从路由事件处理程序中移除路由事件
            remove { RemoveHandler(ClickEvent, value); }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            RoutedEventArgs args2 = new RoutedEventArgs(ClickEvent, this);
            //引用自定义路由事件
            this.RaiseEvent(args2);
        }
    }
}