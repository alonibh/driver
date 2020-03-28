using Driver.API;
using Driver.Helpers;
using Driver.Models;
using GalaSoft.MvvmLight.Views;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class SearchPersonViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private readonly IDialogService _dialogService;
        private readonly IDbHelper _dbHelper;
        private ObservableCollection<Friend> _searchResults = new ObservableCollection<Friend>();


        public ICommand PerformSearch => new Command<string>(async (string query) => await SearchPerson(query));
        public ObservableCollection<Friend> SearchResults
        {
            get
            {
                return _searchResults;
            }
            set
            {
                _searchResults = value;
                OnPropertyChanged();
            }
        }

        public SearchPersonViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _dbHelper = DependencyService.Get<IDbHelper>();
            _dialogService = DependencyService.Get<IDialogService>();
        }

        internal async void OnTextChanged(object sender, TextChangedEventArgs e) =>
            await SearchPerson(e.NewTextValue);

        internal async void OnFriendTapped(object sender, ItemTappedEventArgs args)
        {
            ListView lv = (ListView)sender;
            lv.SelectedItem = null;

            Friend friend = (args.Item as Friend);
            bool answer = await _dialogService.ShowMessage("Add Friend", $"Do you want to send a friend request to {friend.FullName}", "Yes", "No", null);
            if (answer)
            {
                var addDriveResponse = await _dbHelper.AddFriend(new AddFriendRequest
                {
                    Username = friend.Username
                });
                if (!addDriveResponse.Success)
                {
                    await _dialogService.ShowMessage("Unable to add friend", "Error", "OK", null);
                    return;
                }
            }
        }

        async Task SearchPerson(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                SearchResults.Clear();
            }
            else
            {
                var SearchPersonResponse = await _dbHelper.SearchPerson(new SearchPersonRequest
                {
                    Query = query
                });

                SearchResults = new ObservableCollection<Friend>(SearchPersonResponse.Results.Select(o => (Friend)o).ToList());
            }
        }
    }
}