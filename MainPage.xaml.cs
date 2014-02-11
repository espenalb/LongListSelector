using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ZedgeLonglistSelector
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void NavigateToComplexVerticalClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/VerticalPage.xaml", UriKind.Relative));
        }

        private void NavigateToComplexSimpleClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SimpleVerticalPage.xaml", UriKind.Relative));
        }
    }
}