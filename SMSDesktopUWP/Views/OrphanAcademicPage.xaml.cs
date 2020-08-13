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
    public sealed partial class OrphanAcademicPage : Page
    {
        public Orphan InOrphan { get; set; }

        public Academic InAcademic { get; set; }

        private bool isNew;

        public OrphanAcademicPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            EditAcademicParams p = new EditAcademicParams();
            p = (EditAcademicParams)e.Parameter;

            InOrphan = (Orphan)p.Member;

            if (p.Academic != null)
            {
                isNew = false;
                InAcademic = p.Academic;
            }
            else
            {
                isNew = true;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            OrphanMasterDetailPage.contentAcademic.Hide();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Academic outAcademic = new Academic();

            if (!isNew)
            {
                outAcademic = InAcademic;
            }

            if (!isNew)
            {
                outAcademic.AcademicID = InAcademic.AcademicID;
            }
            outAcademic.OrphanID = InOrphan.OrphanID;
            outAcademic.EntryDate = dtEntryDate.Date.Value.DateTime;
            outAcademic.Grade = txtGrade.Text;
            outAcademic.KCPE = txtKCPE.Text;
            outAcademic.KCSE = txtKCSE.Text;
            outAcademic.School = txtSchool.Text;


            // Make sure you validate the above stuff...blows up otherwise.

            if (isNew)
            {
                // Update to Database
                AcademicDataService.AddAcademic(outAcademic);
            }
            else
            {
                // Go get the one of interest, then overwrite.

                // Update to Database
                AcademicDataService.SaveAcademic(InOrphan.OrphanID, outAcademic);

            }

            // Close the page
            OrphanMasterDetailPage.contentAcademic.Hide();
        }

    }
}
