using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using Zedge.Core.ViewModel;

namespace ZedgeLonglistSelector
{
    public class DesigntimeData
    {
        public NavigationService NavigationService { get; set; }
        public ItemVerticalViewModel RingtoneVertical
        {
            get
            {
                var model = new ItemVerticalViewModel(null, null);
                foreach (var sectionName in new[] { "Featured", "Recent", "Popular"})
                    model.Sections.Add(CreateSectionList(sectionName));
                return model;
            }
        }

        private ContentListModel CreateSectionList(string name)
        {
            return new ContentListModel(NavigationService, null, null)
            {
                Name = name,
                Items =
                {
                    CreateItem(name,0,"Page1"),
                    CreateItem(name,1,"Page1"),
                    CreateItem(name,2,"Page1"),
                    CreateItem(name,3,"Page1"),
                },
                LoadNextPageFunction = async cursor =>
                {
                    var newItems = new List<ContentItemModel>();
                    for (var ix = 0; ix < 10; ix++)
                        newItems.Add(CreateItem(name, ix, cursor));
                    await Task.Delay(1500); // <-- Fake a delay from HttpRequest
                    return newItems;
                }
            };
        }

        private ContentItemModel CreateItem(string name, int ix, string cursor)
        {
            return new ContentItemModel
            {
                Id = ix, 
                Title = name + cursor + " Ring" + ix,
            };
        }
    }

    internal class NavigateToItemCommand : ICommand
    {
        private readonly NavigationService _navigationService;

        public NavigateToItemCommand(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _navigationService.Navigate(new Uri("/ItemDetailsView.xaml?" + parameter, UriKind.Relative));
        }

        public event EventHandler CanExecuteChanged;
    }
}