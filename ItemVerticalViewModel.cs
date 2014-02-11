using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Zedge.Core.ViewModel
{
    public class ItemVerticalViewModel : INotifyPropertyChanged
    {
        #region Fields and properties

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IContentProvider _contentProvider;
        private int _defaultSectionIndex;
        private readonly NavigationService _navigationService;
        public ObservableCollection<ContentListModel> Sections { get; private set; }

        #endregion

        public string Name { get; set; }

        public ContentTypeInfo ContentInfo { private get; set; }

        public int DefaultSectionIndex
        {
            get { return _defaultSectionIndex; }
            set
            {
                if (value == _defaultSectionIndex) return;
                _defaultSectionIndex = value;
                OnPropertyChanged();
            }
        }
        
        public ItemVerticalViewModel(IContentProvider contentProvider, NavigationService navigationService)
        {
            _contentProvider = contentProvider;
            _navigationService = navigationService;
            Sections = new ObservableCollection<ContentListModel>();
        }

        public async Task<bool> LoadAsync(ContentTypeInfo contentTypeInfo)
        {
            try
            {
                _logger.Debug("LoadAsync {0} started", Name);
                ContentInfo = contentTypeInfo;
                return await LoadAsync();
            }
            finally
            {
                _logger.Debug("LoadAsync {0} completed", Name);
            }
        }
        private async Task<bool> LoadAsync()
        {
            if (ContentInfo == null)
                throw new NotSupportedException("Cannot load when contentinfo not set");
            if (_contentProvider == null)
                throw new NotSupportedException("Cannot load when _contentProvider not set");
            var loadContentTasks = new List<Task<int>>();
            Sections.Clear();
            foreach (var section in ContentInfo.Sections)
            {
                var section1 = section;
                var contentList = new ContentListModel(_navigationService, _contentProvider, ContentInfo)
                {
                    Name = section.Label,
                    LoadNextPageFunction = async cursor => await _contentProvider.ListItemsAsync(ContentInfo, section1, cursor, ContentInfo.ItemLimit).ConfigureAwait(false),
                };
                loadContentTasks.Add(contentList.LoadAsync(_navigationService));
                Sections.Add(contentList);
            }
          
            // Now load the data
            var itemsLoaded = 0;
            foreach (var loadTask in loadContentTasks)
            {
                var count = await loadTask;
                itemsLoaded += count;
                _logger.Debug("Loaded {0} items - total itemsLoaded now {1}", count, itemsLoaded);
                await Task.Yield(); // Give UI time to update before loading next section...
            }
            return await Task.FromResult(true);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface INavigationService
    {
    }

    public interface IContentProvider
    {
        Task<IList<ContentItemModel>> ListItemsAsync(ContentTypeInfo contentInfo, Section section, string cursor, int itemLimit);
    }

    internal class LogManager
    {
        public static ILogger GetCurrentClassLogger()
        {
            return new Logger();
        }

        private class Logger : ILogger
        {
            public void Debug(string format, params object[] args)
            {
                Console.WriteLine(format,args);
            }
        }
    }

    internal interface ILogger
    {
        void Debug(string format, params object[] args);
    }

    public class ContentTypeInfo
    {
        public IEnumerable<Section> Sections { get; set; }
        public int ItemLimit { get; set; }
    }

    public class Section
    {
        public string Label { get; set; }

    }
}