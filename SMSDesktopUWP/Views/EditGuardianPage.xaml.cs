using SMSDesktopUWP.Core.Models;
using SMSDesktopUWP.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SMSDesktopUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditGuardianPage : Page
    {

        private Guardian _inGuardian;

        private bool isNew = true;

        public Guardian InGuardian
        {
            get { return _inGuardian; }
            set { _inGuardian = value; }
        }

        public EditGuardianPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                InGuardian = (Guardian)e.Parameter;

                isNew = false;
            }
            else
            {
                isNew = true;
            }

            base.OnNavigatedTo(e);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            On_BackRequested();
        }

        // Handles system-level BackRequested events and page-level back button Click events
        private bool On_BackRequested()
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                return true;
            }
            return false;
        }

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Guardian outGuardian = new Guardian();

            if (!isNew)
            {
                outGuardian = InGuardian;
            }

            outGuardian.FirstName = txtFirstName.Text;
            outGuardian.LastName = txtLastName.Text;
            outGuardian.FullName = txtFirstName.Text + " " + txtLastName.Text;
            outGuardian.Location = txtLocation.Text;

            if (isNew)
            {
                // Update to Database
                GuardianDataService.AddGuardian(outGuardian);
            }
            else
            {
                // Go get the one of interest, then overwrite.

                // Update to Database
                GuardianDataService.SaveGuardian(outGuardian);

            }

            // Go Back
            On_BackRequested();

        }


    }
}
