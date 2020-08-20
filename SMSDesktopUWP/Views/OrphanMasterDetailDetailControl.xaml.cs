using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using SMSDesktopUWP.Core.HttpRepository;
using SMSDesktopUWP.Core.Models;
using SMSDesktopUWP.Core.Services;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

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

        private async void UploadPicOnClick(object sender, RoutedEventArgs e)
        {
            await UploadPicAsync();
        }

        private async Task UploadPicAsync()
        {
            var picker = new FileOpenPicker();

            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".gif");

            StorageFile file = await picker.PickSingleFileAsync();

            if (file == null) return;

            using (var client = new HttpClient())
            using (var fileStream = await file.OpenReadAsync())
            {
                var pictureCreation = new PictureCreation
                {
                    PictureFileName = file.Name,
                    Caption = string.Empty,
                    SetAsProfilePic = true,
                    OrphanID = MasterMenuItem.OrphanID
                };

                var picRepository = new PictureHttpRepository(client);

                if (AppSettings.UseWebApi)
                {
                    MasterMenuItem.ProfilePicUri = await picRepository.UploadImageAsync(pictureCreation, fileStream.AsStreamForRead());
                    //
                    // TODO re-render the view
                    //
                    Uri uri = new Uri(MasterMenuItem.ProfilePicUri);
                    BitmapImage img = new BitmapImage(uri);
                    imgProfilePic.Source = img;
                }
                else // Direct to db
                {
                    MasterMenuItem.ProfilePicUri = await picRepository.UploadImageDemoAsync(pictureCreation, fileStream.AsStreamForRead());
                    //
                    // TODO re-render the view
                    //
                    Uri uri = new Uri(MasterMenuItem.ProfilePicUri);
                    BitmapImage img = new BitmapImage(uri);
                    imgProfilePic.Source = img;
                }
            }
        }

        private async void Image_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            await UploadPicAsync();
        }
    }
}
