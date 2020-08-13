using System;

using SMSDesktopUWP.Core.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SMSDesktopUWP.Views
{
    public sealed partial class GuardianMasterDetailDetailControl : UserControl
    {
        public Guardian MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as Guardian; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(Guardian), typeof(GuardianMasterDetailDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        public GuardianMasterDetailDetailControl()
        {
            InitializeComponent();
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as GuardianMasterDetailDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
