using IniFile;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace FrpGui {

    /// <summary>
    /// IniItemControl.xaml 的交互逻辑
    /// </summary>
    public partial class IniItemControl : UserControl {

        public IniItemControl() {
            InitializeComponent();
            //grid.DataContext = this;
            dockPanel.DataContext = this;
            //Value = "123";
        }

        public string Comment {
            get {
                if (Comments == null) {
                    return "";
                }
                if (!Comments.Any()) {
                    return "";
                }
                return Comments.Select(v => v.Text).Aggregate((longest, next) => {
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

        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register("Key", typeof(string), typeof(IniItemControl), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public PropertyValue Value {
            get {
                return (PropertyValue)GetValue(ValueProperty);
            }
            set {
                SetValue(ValueProperty, value);
                //loadData();
            }
        }

        

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(PropertyValue), typeof(IniItemControl), new FrameworkPropertyMetadata(default(PropertyValue), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e) {
            base.OnPropertyChanged(e);
        }
        static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var obj = (IniItemControl)d;
        }

        public bool IsEditor { get; set; }

        public static readonly DependencyProperty IsEditorProperty = DependencyProperty.Register("IsEditor", typeof(bool), typeof(IniItemControl), new PropertyMetadata(null));

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

        private void textBox__value_TextChanged(object sender, TextChangedEventArgs e) {
            //var text_box = sender as TextBox;
            //Value = text_box.Text;
            //Value = "123";
            //SetValue(ValueProperty, Value);
        }
    }
}
