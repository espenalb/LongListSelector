﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly Stopwatch _stopWatch = new Stopwatch();
        // Constructor
        public VerticalViewPage()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _stopWatch.Stop();

            LoadTime.Text = String.Format("Page loaded in {0}ms", _stopWatch.ElapsedMilliseconds);
        }

        private async void OnItemRealized(object sender, ItemRealizationEventArgs e)
        {
            var selector = sender as FrameworkElement;
            var list = selector.DataContext as ContentListModel;
            await list.OnItemRealized(e.Container.Content as ContentItemModel);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            _stopWatch.Restart();
            if (e.NavigationMode == NavigationMode.Back)
                return;
            base.OnNavigatedTo(e);
            var dataProvider = new DesigntimeData
            {
                NavigationService = this.NavigationService,
            };
            DataContext = dataProvider.RingtoneVertical;
            foreach (var section in ViewModel.Sections)
            {
                await section.LoadAsync(this.NavigationService);
            }
        }

        private ItemVerticalViewModel ViewModel
        {
            get { return DataContext as ItemVerticalViewModel; }
        }
    }
}