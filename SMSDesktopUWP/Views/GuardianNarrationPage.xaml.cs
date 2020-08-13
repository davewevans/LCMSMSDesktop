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
    public sealed partial class GuardianNarrationPage : Page
    {
        public Narration InNarration { get; set; }

        public Guardian InGuardian { get; set; }

        private bool isNew;

        public GuardianNarrationPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            EditNarrationParams p = new EditNarrationParams();
            p = (EditNarrationParams)e.Parameter;
            InGuardian = (Guardian)p.Member;

            if (p.Narration != null)
            {
                isNew = false;
                InNarration = p.Narration;
            }
            else
            {
                isNew = true;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            GuardianMasterDetailPage.contentNarration.Hide();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Narration outNarration = new Narration();

            if (!isNew)
            {
                outNarration = InNarration;
            }

            if (!isNew)
            {
                outNarration.NarrationID = InNarration.NarrationID;
            }
            outNarration.GuardianID = InGuardian.GuardianID;
            outNarration.EntryDate = dtEntryDate.Date.Value.DateTime;
            outNarration.Subject = txtSubject.Text;
            outNarration.Note = txtNarration.Text;

            // Make sure you validate the above stuff...blows up otherwise.

            if (isNew)
            {
                // Update to Database
                NarrationDataService.AddNarration(outNarration);
            }
            else
            {
                // Go get the one of interest, then overwrite.

                // Update to Database
                NarrationDataService.SaveNarration(InGuardian.GuardianID, outNarration);

            }

            // Close the page
            GuardianMasterDetailPage.contentNarration.Hide();
        }


    }
}
