using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Zedge.Core.ViewModel;
using ZedgeLonglistSelector.Resources;

namespace ZedgeLonglistSelector
{
    public partial class VerticalViewPage : PhoneApplicationPage
    {
        // Constructor
        public VerticalViewPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
        private async void OnItemRealized(object sender, ItemRealizationEventArgs e)
        {
            var selector = sender as FrameworkElement;
            var list = selector.DataContext as ContentListModel;
            await list.OnItemRealized(e.Container.Content as ContentItemModel);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
                return;
            base.OnNavigatedTo(e);
            var dataProvider = new DesigntimeData();
            DataContext = dataProvider.RingtoneVertical;
            foreach (var section in ViewModel.Sections)
            {
                await section.LoadAsync(this.NavigationService);
            }
        }

        private ItemVerticalViewModel ViewModel {
            get { return DataContext as ItemVerticalViewModel; }
        }
    }
}