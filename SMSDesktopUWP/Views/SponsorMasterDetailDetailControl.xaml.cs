using System;

using SMSDesktopUWP.Core.Models;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SMSDesktopUWP.Views
{
    public sealed partial class SponsorMasterDetailDetailControl : UserControl
    {
        public SampleOrder MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as SampleOrder; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(SampleOrder), typeof(SponsorMasterDetailDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        public SponsorMasterDetailDetailControl()
        {
            InitializeComponent();
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SponsorMasterDetailDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
