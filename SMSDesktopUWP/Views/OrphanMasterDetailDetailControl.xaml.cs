using System;

using SMSDesktopUWP.Core.Models;
using SMSDesktopUWP.Core.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SMSDesktopUWP.Views
{
    public sealed partial class OrphanMasterDetailDetailControl : UserControl
    {
        public delegate void OrphanEventHandler(object source, EventArgs e);

        public event OrphanEventHandler OnNavigateParentReady;

        public Orphan MasterMenuItem
        {
            //get { return GetValue(MasterMenuItemProperty) as SampleOrder; }
            get { return GetValue(MasterMenuItemProperty) as Orphan; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public char SymbolDisplay
        {
            get { return (char)57661; }
        }

        //public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(SampleOrder), typeof(OrphanMasterDetailDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));
        public static readonly DependencyProperty MasterMenuItemProperty =
            DependencyProperty.Register("MasterMenuItem", typeof(Orphan), typeof(OrphanMasterDetailDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        public OrphanMasterDetailDetailControl()
        {
            InitializeComponent();
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as OrphanMasterDetailDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //((Frame)Window.Current.Content).Navigate(typeof(Views.NewOrphanPage));
            OnNavigateParentReady(this, null);
        }
    }
}
