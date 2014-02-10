using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Navigation;
using ZedgeLonglistSelector;

namespace Zedge.Core.ViewModel
{
    public class ContentListModel : INotifyPropertyChanged
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        protected ContentTypeInfo ContentTypeInfo { get; private set; }
        protected NavigationService NavigationService { get; private set; }
        private IContentProvider ContentProvider { get; set; }
        private string _name;

        public ContentListModel(NavigationService navigationService, IContentProvider contentProvider,
            ContentTypeInfo contentTypeInfo)
        {
            ContentTypeInfo = contentTypeInfo;
            NavigationService = navigationService;
            ContentProvider = contentProvider;
            Items = new ObservableCollection<ContentItemModel>();

            //TODO: Get this from config
            LoadNextPageThreshold = 1;
        }

        public String Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ContentItemModel> Items { get; private set; }

        public bool IsFinite { private get; set; }
        public int LoadNextPageThreshold { private get; set; }
        public int CurrentPage { get; private set; }

        private void AddItems(IEnumerable<ContentItemModel> contentList)
        {
            foreach (var item in contentList)
            {
                item.NavigateToItemCommand = new NavigateToItemCommand(_navigationService);
                Items.Add(item);
            }
            _logger.Debug("AddItems completed on list {0}. Now contains {1} items", Name, Items.Count());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }


        /// <summary>
        /// Loads next items when required
        /// </summary>
        /// <param name="contentItemModel"></param>
        /// <returns></returns>
        public async Task<int> OnItemRealized(ContentItemModel contentItemModel)
        {
            if (!ShouldLoadMoreItems(contentItemModel)) // Do nothing in this event
            {
                return 0;
            }

            //_logger.Debug("OnItemRealized-{0} Index {1} of {2}", Name, index, Items.Count);
            return await LoadNextPage();
        }

        private bool ShouldLoadMoreItems(ContentItemModel contentItemModel)
        {
            if (IsFinite || Loading)
                return false;

            var index = Items.IndexOf(contentItemModel);
            return Items.Count - index <= LoadNextPageThreshold;
        }

        public bool Loading
        {
            get { return _loading; }
            set
            {
                if (value.Equals(_loading)) return;
                _loading = value;
                OnPropertyChanged();
            }
        }

        public string LoadingText
        {
            get { return "Loading..."; }
        }

        private async Task<int> LoadNextPage()
        {
            try
            {
                Loading = true;

                if (LoadNextPageFunction == null) throw new NotSupportedException("LoadNextPage must be set!");

                _logger.Debug("LoadNextPageIfNotAllreadyLoading - loading page {0} for {1}", CurrentPage, Name);

                var contentList = await LoadNextPageFunction(GetCursor());

                AddItems(contentList);
                CurrentPage++;
                return contentList.Count;
            }
            finally
            {
                Loading = false;
            }
        }

        private string GetCursor()
        {
            return "Page" + CurrentPage;
        }


        public Func<string, Task<IList<ContentItemModel>>> LoadNextPageFunction;
        private bool _loading;
        private NavigationService _navigationService;

        public async Task<int> LoadAsync(NavigationService navigationService)
        {
            _navigationService = navigationService;
            CurrentPage = 0;
            Items.Clear();
            return await LoadNextPage();
        }
    }

    internal class LoadNextPageException : Exception
    {
        public LoadNextPageException(string message)
            : base(message)
        {
        }
    }
}