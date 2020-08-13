using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Microsoft.Toolkit.Uwp.UI.Controls;

using SMSDesktopUWP.Core.Models;
using SMSDesktopUWP.Core.Services;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace SMSDesktopUWP.Views
{
    public sealed partial class GuardianMasterDetailPage : Page, INotifyPropertyChanged
    {
        //private SampleOrder _selected;
        private Guardian _selected;

        private Narration selectedNote;

        public List<Guardian> guardianList = new List<Guardian>();
        List<Guardian> listGuardianSuggestion = null;

        public ObservableCollection<Narration> NarrationItems { get; private set; } = new ObservableCollection<Narration>();

        public static ContentDialog contentNarration;

        public Guardian Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ObservableCollection<Guardian> GuardianItems { get; private set; } = new ObservableCollection<Guardian>();

        public GuardianMasterDetailPage()
        {
            InitializeComponent();
            Loaded += GuardianMasterDetailPage_Loaded;
        }

        private async void GuardianMasterDetailPage_Loaded(object sender, RoutedEventArgs e)
        {
            GuardianItems.Clear();

            var data = await GuardianDataService.AllGuardians();

            foreach (var item in data)
            {
                GuardianItems.Add(item);
            }

            if (MasterDetailsViewControl.ViewState == MasterDetailsViewState.Both)
            {
                Selected = GuardianItems.FirstOrDefault();
            }
        }

        private async void LoadGuardians()
        {
            GuardianItems.Clear();

            var data = await GuardianDataService.AllGuardians();

            foreach (var item in data)
            {
                GuardianItems.Add(item);
            }

            if (MasterDetailsViewControl.ViewState == MasterDetailsViewState.Both)
            {
                Selected = GuardianItems.FirstOrDefault();
            }
        }

        private void LoadNarrationItems()
        {
            foreach (var nar in Selected.Narrations)
            {
                NarrationItems.Add(nar);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
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
            guardianList = GuardianItems.ToList();

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
                listGuardianSuggestion = guardianList.Where(o => o.FullName.ToLower().Contains(sender.Text.ToLower())).ToList();
                sender.ItemsSource = listGuardianSuggestion;

                //
                // Added this to refresh the items source
                //
                MasterDetailsViewControl.ItemsSource = listGuardianSuggestion;

            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var searchTerm = args.QueryText;
            var results = guardianList.Where(i => i.FullName.Contains(searchTerm)).ToList();
            sender.ItemsSource = results;
            sender.IsSuggestionListOpen = true;
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            // Set sender.Text. You can use args.SelectedItem to build your text string.
            var selectedItem = args.SelectedItem as Guardian;
            sender.Text = selectedItem.FullName;

            Selected = selectedItem;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EditGuardianPage));
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EditGuardianPage), Selected);
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Delete confirmation
            if (Selected != null)
            {
                ContentDialog notifyDelete = new ContentDialog()
                {
                    Title = "Confirm delete?",
                    Content = "Are you sure you wish to delete " + Selected.FullName + "?",
                    PrimaryButtonText = "Delete Guardian",
                    SecondaryButtonText = "Cancel"

                };

                ContentDialogResult result = await notifyDelete.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    // Delete
                    GuardianDataService.DeleteGuardian(Selected);

                    // Clear search text box
                    // Can't get to it because it's inside a data template!

                    // Repopulate the list
                    LoadGuardians();
                }
                else
                {
                    // User pressed Cancel or the back arrow.
                }

            }

        }

        private async void btnNotes_Click(object sender, RoutedEventArgs e)
        {
            contentNarration = new ContentDialog();

            Frame frmNarration = new Frame();

            EditNarrationParams p = new EditNarrationParams();
            p.Source = EditSource.Guardian;
            p.Member = Selected;
            p.Narration = null;

            frmNarration.Navigate(typeof(GuardianNarrationPage), p);

            contentNarration.Content = frmNarration;

            await contentNarration.ShowAsync();

            //LoadNarrationItems();
            LoadGuardians();

        }

        private void NotesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedNote = (Narration)(sender as ListView).SelectedItem;
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
                p.Source = EditSource.Guardian;
                p.Member = Selected;
                p.Narration = (Narration)selectedNote;

                frmNarration.Navigate(typeof(GuardianNarrationPage), p);

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
                    LoadGuardians();
                }
                else
                {
                    // User pressed Cancel or the back arrow.
                }

            }
        }
    }
}
