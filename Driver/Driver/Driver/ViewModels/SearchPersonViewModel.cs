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
        private readonly IDialogService _dialogService;
        private readonly IDbHelper _dbHelper;

        private ObservableCollection<Friend> _searchResults;
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

        public ICommand PerformSearch => new Command<string>(async (string query) => await SearchPerson(query));

        public SearchPersonViewModel()
        {
            _dbHelper = DependencyService.Get<IDbHelper>();
            _dialogService = DependencyService.Get<IDialogService>();
            _searchResults = new ObservableCollection<Friend>();
        }

        public async void OnTextChanged(object sender, TextChangedEventArgs e) =>
            await SearchPerson(e.NewTextValue);

        public async void OnFriendTapped(object sender, ItemTappedEventArgs args)
        {
            ListView lv = (ListView)sender;
            lv.SelectedItem = null;

            Friend friend = (args.Item as Friend);
            bool answer = await _dialogService.ShowMessage($"Do you want to send a friend request to {friend.FullName}", "Add Friend", "Yes", "No", null);
            if (answer)
            {
                await _dbHelper.AddFriend(new AddFriendRequest
                {
                    Username = friend.Username
                });
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