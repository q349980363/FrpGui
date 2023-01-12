using IniFile;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FrpGui {

    /// <summary>
    /// IniSectionControl.xaml 的交互逻辑
    /// </summary>
    public partial class IniSectionControl : UserControl {

        public IniSectionControl() {
            InitializeComponent();
            groupBox.DataContext = grid.DataContext = this;
            loadData();
        }

        private Ini ini;

        public Section Section {
            get {
                return (Section)GetValue(SectionProperty);
            }
            set {
                SetValue(SectionProperty, value);
                loadData();
            }
        }

        public static readonly DependencyProperty SectionProperty = DependencyProperty.Register("Section", typeof(Section), typeof(IniSectionControl), new PropertyMetadata(PropertyChangedCallback));

        public BindingList<IniFile.Property> KeyList { get; set; } = new BindingList<IniFile.Property>();

        static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            //当项名被设置时  加载名称
            if (e.Property == SectionProperty) {
                var obj = (IniSectionControl)d;
                obj.loadData();
            }
        }

        public void loadData() {
            KeyList.Clear();
            if (Section == null) {
                return;
                //KeyList[0].Comments
                //var a = KeyList[0].Comments.Aggregate(v => v.Text);
            }
            foreach (var key in Section) {
                KeyList.Add(key);
            }
        }


        //编辑
        private void Button_Click(object sender, RoutedEventArgs e) {
            //编辑也要跳转到上级才行
            RoutedEventArgs args2 = new RoutedEventArgs(ClickEditorEvent, this);
            //引用自定义路由事件
            this.RaiseEvent(args2);
        }

        public static readonly RoutedEvent ClickDelEvent =
        EventManager.RegisterRoutedEvent("ClickDelEvent", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventArgs<Object>), typeof(IniSectionControl));

        public event RoutedEventHandler ClickDel {
            //将路由事件添加路由事件处理程序
            add { AddHandler(ClickDelEvent, value); }
            //从路由事件处理程序中移除路由事件
            remove { RemoveHandler(ClickDelEvent, value); }
        }

        public static readonly RoutedEvent ClickEditorEvent =
        EventManager.RegisterRoutedEvent("ClickEditorEvent", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventArgs<Object>), typeof(IniSectionControl));

        public event RoutedEventHandler ClickEditor {
            //将路由事件添加路由事件处理程序
            add { AddHandler(ClickEditorEvent, value); }
            //从路由事件处理程序中移除路由事件
            remove { RemoveHandler(ClickEditorEvent, value); }
        }

        //跳转上级
        private void Button_Click_1(object sender, RoutedEventArgs e) {
            RoutedEventArgs args2 = new RoutedEventArgs(ClickDelEvent, this);
            //引用自定义路由事件
            this.RaiseEvent(args2);
        }
    }
}