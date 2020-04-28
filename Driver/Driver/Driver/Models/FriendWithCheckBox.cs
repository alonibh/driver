using System.ComponentModel;

namespace Driver.Models
{
    public class FriendWithCheckBox : INotifyPropertyChanged
    {
        public Friend Friend { get; set; }
        private bool _isChecked;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs(null));
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}