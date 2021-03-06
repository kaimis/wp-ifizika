﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ifizika.Resources;
using ifizika.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace ifizika
{
    public partial class MainPage : PhoneApplicationPage
    {
        private bool _isDataLoaded;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            if (_isDataLoaded == false)
            {
                App.ThemesViewModel.Items.Clear();
            }
            BuildLocalizedApplicationBar();
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            _isDataLoaded = true;
        }
        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ClassList.DataContext = App.ViewModel.Items;
            this.Loaded += DataLoaded;
        }
        // Build a localized ApplicationBar
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar
            {
                Mode = ApplicationBarMode.Minimized
            };

            // Create a new menu item with the localized string from AppResources.
            var appBarMenuItemContacts = new ApplicationBarMenuItem(AppResources.Contacts);
            appBarMenuItemContacts.Click += Contacts;
            var appBarMenuItemFeedback = new ApplicationBarMenuItem(AppResources.Feedback);
            appBarMenuItemFeedback.Click += Feedback;
            var appBarMenuItemSettings = new ApplicationBarMenuItem(AppResources.Settings);
            appBarMenuItemSettings.Click += SettingsMenu;

            ApplicationBar.MenuItems.Add(appBarMenuItemContacts);
            ApplicationBar.MenuItems.Add(appBarMenuItemFeedback);
            ApplicationBar.MenuItems.Add(appBarMenuItemSettings);
        }

        private void DataLoaded(object sender, RoutedEventArgs e)
        {
            if (_isDataLoaded == false)
            {
                var prog = new ProgressIndicator
                {
                    IsVisible = true,
                    IsIndeterminate = true,
                    Text = AppResources.LoadingData
                };
                SystemTray.SetProgressIndicator(this, prog);

                App.ViewModel.LoadData();
            }
        }
        // Handle selection changed on ListBox
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (ClassList.SelectedIndex == -1)
                return;

            // Navigate to the new page
            var tempClassModel = ClassList.SelectedItem as ClassModel;
            if (tempClassModel != null)
                NavigationService.Navigate(new Uri("/Pages/Categories.xaml?selectedItem=" + tempClassModel.Class + "&name=" + tempClassModel.Name, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            ClassList.SelectedIndex = -1;
        }

        private void Contacts(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Contacts.xaml", UriKind.Relative));
        }

        private void Feedback(object sender, EventArgs e)
        {
            var marketplaceReviewTask = new MarketplaceReviewTask();

            marketplaceReviewTask.Show();
        }

        private void SettingsMenu(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Settings.xaml", UriKind.Relative));
        }
    }
}