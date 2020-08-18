using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Triggers;
using SMSDesktopUWP.Core.HttpRepository;
using SMSDesktopUWP.Core.Models;
using SMSDesktopUWP.Core.Services;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace SMSDesktopUWP.Views
{
    public sealed partial class OrphanMasterDetailPage : Page, INotifyPropertyChanged
    {
        //private SampleOrder _selected;
        private Orphan _selected;

        private Narration selectedNote;
        private Academic selectedAcademic;

        //list for AutoSuggestBox
        public List<Orphan> orphanList = new List<Orphan>();
        List<Orphan> listOrphanSuggestion = null;

        public static ContentDialog contentNarration;
        public static ContentDialog contentAcademic;

        public Orphan Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ObservableCollection<Orphan> OrphanItems { get; private set; } = new ObservableCollection<Orphan>();
        public ObservableCollection<Narration> NarrationItems { get; private set; } = new ObservableCollection<Narration>();
        public ObservableCollection<Academic> AcademicItems { get; private set; } = new ObservableCollection<Academic>();

        public OrphanMasterDetailPage()
        {
            InitializeComponent();
            Loaded += OrphanMasterDetailPage_Loaded;
        }

        private async void OrphanMasterDetailPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadOrphansAsync();

            if (MasterDetailsViewControl.ViewState == MasterDetailsViewState.Both)
            {
                Selected = OrphanItems.FirstOrDefault();
            }
        }

        private async Task LoadOrphansAsync()
        {
            //SampleItems.Clear();
            OrphanItems.Clear();

            IEnumerable<Orphan> data;

            //
            // Use Web API or db directly
            //
            if (AppSettings.UseWebApi)
            {
                using (HttpClient client = new HttpClient())
                {
                    var orphanRepo = new OrphanHttpRepository(client);
                    var parameters = new OrphanParametes
                    {
                        PageNumber = 1,
                        PageSize = 1000
                    };
                    var response = await orphanRepo.GetOrphansAsync(parameters);
                    data = response.Orphans.AsEnumerable();
                }
            }
            else
            {
                data = await OrphanDataService.AllOrphans();

                using (HttpClient client = new HttpClient())
                {
                    var orphanRepo = new PictureHttpRepository(client);
                    var picUrls = await orphanRepo.GetOrphanPicUrls();

                    foreach (var item in picUrls)
                    {
                        var orphan = data.FirstOrDefault(x => x.OrphanID == item.OrphanID);
                        if (orphan != null)
                        {
                            orphan.ProfilePicUri = item.PicUrl;
                        }                  
                    }
                }
              
            }

            //var data = await SampleDataService.GetMasterDetailDataAsync();
            //var data = await OrphanDataService.AllOrphans();

            foreach (var item in data)
            {
                //SampleItems.Add(item);
                OrphanItems.Add(item);
            }

            if (MasterDetailsViewControl.ViewState == MasterDetailsViewState.Both)
            {

                Selected = OrphanItems.FirstOrDefault();

                LoadNarrationItems();
            }
        }

        private void LoadNarrationItems()
        {
            if (Selected == null) return;

            foreach (var nar in Selected.Narrations)
            {
                NarrationItems.Add(nar);
            }
        }

        private void LoadAcademicItems()
        {
            if (Selected == null) return;

            foreach (var aca in Selected.Academics)
            {
                AcademicItems.Add(aca);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void AutoSuggestionBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            orphanList = OrphanItems.ToList();

            // Only get results when it was a user typing,
            // otherwise assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //
                // Boom:  David nailed this.
                //
                // Added ToLower() calls to normalize text 
                //
                //Set the ItemsSource to be your filtered dataset
                listOrphanSuggestion = orphanList.Where(o => o.FullName.ToLower().Contains(sender.Text.ToLower())).ToList();
                sender.ItemsSource = listOrphanSuggestion;

                //
                // Added this to refresh the items source
                //
                MasterDetailsViewControl.ItemsSource = listOrphanSuggestion;

            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            // Set sender.Text. You can use args.SelectedItem to build your text string.
            var selectedItem = args.SelectedItem as Orphan;
            sender.Text = selectedItem.FullName;

            Selected = selectedItem;
            LoadNarrationItems();

        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

            var searchTerm = args.QueryText;
            var results = orphanList.Where(i => i.FullName.Contains(searchTerm)).ToList();
            sender.ItemsSource = results;
            sender.IsSuggestionListOpen = true;
        }

        private void btnAdd_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EditOrphanPage));
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EditOrphanPage), Selected);
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Delete confirmation
            if (Selected != null)
            {
                ContentDialog notifyDelete = new ContentDialog()
                {
                    Title = "Confirm Orphan delete?",
                    Content = "Are you sure you wish to delete " + Selected.FullName + "?",
                    PrimaryButtonText = "Delete Orphan",
                    SecondaryButtonText = "Cancel"

                };

                ContentDialogResult result = await notifyDelete.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {

                    if (AppSettings.UseWebApi)
                    {
                        using (var client = new HttpClient())
                        {
                            var orphanRepo = new OrphanHttpRepository(client);
                            await orphanRepo.DeleteOrphanAsync(Selected.OrphanID);
                        }
                    }
                    else
                    {
                        // Delete
                        OrphanDataService.DeleteOrphan(Selected);
                    }
                   

                    // Clear search text box
                    // Can't get to it because it's inside a data template!

                    // Repopulate the list
                    LoadOrphansAsync();
                }
                else
                {
                    // User pressed Cancel or the back arrow.
                    
                }
            }
        }

        private async void btnAcademics_Click(object sender, RoutedEventArgs e)
        {
            contentAcademic = new ContentDialog();

            Frame frmAcademic = new Frame();

            EditAcademicParams p = new EditAcademicParams();
            p.Source = EditSource.Orphan;
            p.Member = Selected;
            p.Academic = null;

            frmAcademic.Navigate(typeof(OrphanAcademicPage), p);
            contentAcademic.Content = frmAcademic;

            await contentAcademic.ShowAsync();

            LoadOrphansAsync();
        }

        private async void btnNotes_Click(object sender, RoutedEventArgs e)
        {
            contentNarration = new ContentDialog();

            Frame frmNarration = new Frame();

            EditNarrationParams p = new EditNarrationParams();
            p.Source = EditSource.Orphan;
            p.Member = Selected;
            p.Narration = null;

            frmNarration.Navigate(typeof(OrphanNarrationPage), p);

            contentNarration.Content = frmNarration;

            await contentNarration.ShowAsync();

            //LoadNarrationItems();
            LoadOrphansAsync();
        }

        private void btnPictures_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void AcademicListView_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            ListView lv = (ListView)sender;

            selectedAcademic = (Academic)lv.SelectedItem;

            if (lv.SelectedItem != null)
            {
                contentAcademic = new ContentDialog();

                Frame frmAcademic = new Frame();

                EditAcademicParams p = new EditAcademicParams();
                p.Source = EditSource.Orphan;
                p.Member = Selected;
                p.Academic = (Academic)selectedAcademic;

                frmAcademic.Navigate(typeof(OrphanAcademicPage), p);

                contentAcademic.Content = frmAcademic;

                await contentAcademic.ShowAsync();

            }
        }

        private async void NotesListView_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            ListView lv = (ListView)sender;

            selectedNote = (Narration)lv.SelectedItem;
            
            if (lv.SelectedItem != null)
            {
                contentNarration = new ContentDialog();

                Frame frmNarration = new Frame();

                EditNarrationParams p = new EditNarrationParams();
                p.Source = EditSource.Orphan;
                p.Member = Selected;
                p.Narration = (Narration)selectedNote;

                frmNarration.Navigate(typeof(OrphanNarrationPage), p);

                contentNarration.Content = frmNarration;

                await contentNarration.ShowAsync();
            }
        }

        private async void btnDeleteNote_Click(object sender, RoutedEventArgs e)
        {
            
            // Delete confirmation
            if (selectedNote != null)
            {
                ContentDialog notifyDelete = new ContentDialog()
                {
                    Title = "Confirm Narration Delete?",
                    Content = "Are you sure you wish to delete note " + selectedNote.Subject + "?",
                    PrimaryButtonText = "Delete Note",
                    SecondaryButtonText = "Cancel"

                };

                ContentDialogResult result = await notifyDelete.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    // Delete
                    NarrationDataService.DeleteNarration(selectedNote);

                    // Somehow update the display
                    LoadOrphansAsync();
                }
                else
                {
                    // User pressed Cancel or the back arrow.
                }
            }
        }

        private void NotesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedNote = (Narration)(sender as ListView).SelectedItem;

        }

        private async void btnDeleteAcademic_Click(object sender, RoutedEventArgs e)
        {
            // Delete confirmation
            if (selectedAcademic != null)
            {
                ContentDialog notifyDelete = new ContentDialog()
                {
                    Title = "Confirm Academic Record Delete?",
                    Content = "Are you sure you wish to delete academic record for  " + selectedAcademic.EntryDate.ToShortDateString() + "?",
                    PrimaryButtonText = "Delete Record",
                    SecondaryButtonText = "Cancel"

                };

                ContentDialogResult result = await notifyDelete.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    // Delete
                    AcademicDataService.DeleteAcademic(selectedAcademic);

                    // Somehow update the display
                    LoadOrphansAsync();
                }
                else
                {
                    // User pressed Cancel or the back arrow.
                }
            }

        }
    }
}
