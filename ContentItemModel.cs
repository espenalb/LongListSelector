using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Zedge.Core.ViewModel
{
    public class ContentItemModel : INotifyPropertyChanged, IEquatable<ContentItemModel>
    {

        #region Constructors
      
        public ContentItemModel()
        {
        }


        #endregion

        #region Properties

        #region Backing fields
        
        private String _itemDownloadUrl;
        private string _categoryText;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region ContentItem fields

        
        public int Category { get; set; }

        
        public List<string> Tags { get; private set; }


        public int Downloads { get; set; }


        public int Stars { get; set; }


        public string Title { get; set; }


        public int Id { get; set; }

        #endregion


        public string CategoryText
        {
            get { return _categoryText; }
            set
            {
                if (value == _categoryText) return;
                _categoryText = value;
                OnPropertyChanged();
            }
        }


        public String ItemDownloadUrl
        {
            get
            {
                return _itemDownloadUrl;
            }
            set
            {
                if (Equals(value, _itemDownloadUrl)) return;
                _itemDownloadUrl = value;
                OnPropertyChanged();
            }
        }


        public virtual bool DisplayCategory
        {
            get { return false; }
        }


        #endregion

        public ICommand NavigateToItemCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #region IEquatable

        public bool Equals(ContentItemModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(_itemDownloadUrl, other._itemDownloadUrl)
                && Category == other.Category
                && Downloads == other.Downloads
                && Stars == other.Stars
                && string.Equals(Title, other.Title)
                && Id == other.Id
                && string.Equals(CategoryText, other.CategoryText);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is ContentItemModel && Equals((ContentItemModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_itemDownloadUrl != null ? _itemDownloadUrl.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Category;
                hashCode = (hashCode * 397) ^ Downloads;
                hashCode = (hashCode * 397) ^ Stars;
                hashCode = (hashCode * 397) ^ (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Id;
                hashCode = (hashCode * 397) ^ (CategoryText != null ? CategoryText.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(ContentItemModel left, ContentItemModel right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ContentItemModel left, ContentItemModel right)
        {
            return !Equals(left, right);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("ContentItem<Id{0}-'{1}', dl:{2},cat:{3}>", Id, Title, Downloads, Category);
        }
    }


}