using IniFile;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

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

        public string Comment {
            get {
                if (Comments == null) {
                    return "";
                }
                if (!Comments.Any()) {
                    return "";
                }
                return Comments.Select(v=>v.Text).Aggregate((longest, next) => {
                    return longest + next;
                });
            }
        }

        public IEnumerable<Comment> Comments {
            get {
                return (IEnumerable<Comment>)GetValue(CommentsProperty);
            }
            set {
                SetValue(CommentsProperty, value);
                //loadData();
            }
        }

        public static readonly DependencyProperty CommentsProperty = DependencyProperty.Register("Comments", typeof(IEnumerable<Comment>), typeof(IniItemControl), new PropertyMetadata(PropertyChangedCallback));


        public string Key {
            get {
                return (string)GetValue(KeyProperty);
            }
            set {
                SetValue(KeyProperty, value);
                //loadData();
            }
        }

        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register("Key", typeof(string), typeof(IniItemControl), new PropertyMetadata(null));

        public string Value {
            get {
                return (string)GetValue(ValueProperty);
            }
            set {
                SetValue(ValueProperty, value);
                //loadData();
            }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(IniItemControl), new PropertyMetadata(null));

        static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            //d.SetValue(IniDataProperty, e.NewValue);
            var obj = (IniItemControl)d;
            //obj.loadData();
        }

        public bool IsEditor { get; set; }

        public static readonly DependencyProperty IsEditorProperty = DependencyProperty.Register("IsEditor", typeof(bool), typeof(IniItemControl), new PropertyMetadata(null));



        //public static string translation(string txt) {
        //    switch (txt) {
        //        case "type":
        //            return "类型 代理类型\r\n[tcp, udp, http, https, stcp, sudp, xtcp, tcpmux]";

        //        case "server_addr":
        //            return "服务器地址";

        //        case "server_port":
        //            return "服务器端口";

        //        case "token":
        //            return "密钥";

        //        case "local_ip":
        //            return "本地服务 IP \r\n默认值:127.0.0.1";

        //        case "local_port":
        //            return "本地服务端口 \r\n配合 local_ip";

        //        case "remote_port":
        //            return "服务端绑定的端口\r\n用户访问此端口的请求会被转发到 local_ip:local_port";

        //        case "custom_domains":
        //            return "服务器绑定自定义域名\r\n是(和 subdomain 两者必须配置一个)";
        //        case "subdomain":
        //            return "自定义子域名\r\n是(和 custom_domains 两者必须配置一个)";

        //        default:
        //            return txt;
        //    }
        //}
        public static readonly RoutedEvent ClickEvent =
            EventManager.RegisterRoutedEvent("ClickEvent", RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventArgs<Object>), typeof(IniItemControl));

        public event RoutedEventHandler Click {
            //将路由事件添加路由事件处理程序
            add { AddHandler(ClickEvent, value); }
            //从路由事件处理程序中移除路由事件
            remove { RemoveHandler(ClickEvent, value); }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e) {
            RoutedEventArgs args2 = new RoutedEventArgs(ClickEvent, this);
            //引用自定义路由事件
            this.RaiseEvent(args2);
        }
    }
}