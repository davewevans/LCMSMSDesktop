using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;
using SMSDesktopUWP.Core.Models;
using SMSDesktopUWP.Core.Services;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Windows.UI.Xaml.Controls;
using Syncfusion.UI.Xaml.Gauges;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Foundation;

namespace SMSDesktopUWP.Views
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public class ActivePieModel
        {
            public string Category { get; set; }
            public int Value { get; set; }
        }

        public List<ActivePieModel> ActiveSeries { get; set; }

        public int TotalOrphanCount { get; set; }

        public int TotalNarrationCount { get; set; }


        public class NarrationPieModel
        {
            public string Category { get; set; }
            public double Value { get; set; }
        }

        public List<NarrationPieModel> NarrationSeries { get; set; }


        public MainPage()
        {
            InitializeComponent();

            DisplayOrphanStats();

            DisplayNarrationStats();

            //OrphanContactStats();

        }

        private void DisplayOrphanStats()
        {

            SfChart chart = new SfChart() { Header = "Orphan Statistics", FontSize = 18, Height = 300, Width = 500 };

            OrphanStatistics OrphanStats = new OrphanStatistics();
            //GuardianStatistics GuardianStats = new GuardianStatistics();
            //NarrationStatistics NarrationStats = new NarrationStatistics();

            OrphanStats = OrphanDataService.GetOrphanStatistics();

            TotalOrphanCount = OrphanStats.TotalCount;

            ActiveSeries = new List<ActivePieModel>()
                {
                    new ActivePieModel { Category = "Active", Value = OrphanStats.ActiveCount},
                    new ActivePieModel { Category = "Inactive", Value = OrphanStats.InactiveCount},
                    new ActivePieModel { Category = "Unspecified", Value = OrphanStats.UnknownCount },
                    //new ActivePieModel { Category = "Total", Value = OrphanStats.TotalCount}
                };

            //Adding Legends for the chart
            ChartLegend legend = new ChartLegend();
            chart.Legend = legend;

            PieSeries series = new PieSeries()
            {
                ItemsSource = ActiveSeries,
                XBindingPath = "Category",
                YBindingPath = "Value",
                ShowTooltip = true,
                Label = "Values",
                AdornmentsInfo = new ChartAdornmentInfo() { ShowLabel = true },
            };

            chart.Series.Add(series);

            stackOrphanStats.Children.Add(chart);

            TextBlock txtTotal = new TextBlock();
            txtTotal.FontSize = 14;
            txtTotal.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            txtTotal.Text = "Total Orphan Count: " + TotalOrphanCount.ToString();
            stackOrphanStats.Children.Add(txtTotal);

        }

        private void DisplayNarrationStats()
        {
            SfChart chart = new SfChart() { Header = "Narration Statistics", FontSize = 18, Height = 300, Width = 500 };

            GuardianStatistics GuardianStats = new GuardianStatistics();
            NarrationStatistics NarrationStats = new NarrationStatistics();

            NarrationStats = NarrationDataService.GetNarrationStatistics();

            TotalNarrationCount = NarrationStats.TotalNarrationCount;

            ActiveSeries = new List<ActivePieModel>()
                {
                    new ActivePieModel { Category = "Orphans", Value = NarrationStats.OrphanNarrationCount},
                    new ActivePieModel { Category = "Guardians", Value = NarrationStats.GuardianNarrationCount }
                };

            //Adding Legends for the chart
            ChartLegend legend = new ChartLegend();
            chart.Legend = legend;

            PieSeries series = new PieSeries()
            {
                ItemsSource = ActiveSeries,
                XBindingPath = "Category",
                YBindingPath = "Value",
                ShowTooltip = true,
                Label = "Values",
                AdornmentsInfo = new ChartAdornmentInfo() { ShowLabel = true },
            };

            chart.Series.Add(series);

            stackNarrationStats.Children.Add(chart);

            TextBlock txtTotal = new TextBlock();
            txtTotal.FontSize = 14;
            txtTotal.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            txtTotal.Text = "Total Narration Count: " + TotalNarrationCount.ToString();
            stackNarrationStats.Children.Add(txtTotal);

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
    }
}
