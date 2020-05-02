using Driver.API;
using Driver.Helpers;
using Driver.Models;
using Driver.Views;
using GalaSoft.MvvmLight.Views;
using MvvmHelpers;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class FriendPopupViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IDbHelper _dbHelper;

        private Drive _currentDrive;
        public Drive CurrentDrive
        {
            get { return _currentDrive; }
            set
            {
                _currentDrive = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Drive> _drives;
        public ObservableCollection<Drive> Drives
        {
            get { return _drives; }
            set
            {
                _drives = new ObservableCollection<Drive>(value.OrderByDescending(o => o.Date));
                OnPropertyChanged();
            }
        }

        public Friend Friend { get; set; }

        public ICommand OnUnfriendButtonClicked => new Command(async () => await Unfriend());

        public FriendPopupViewModel(Friend friend, List<Drive> drives)
        {
            Friend = friend;
            Drives = new ObservableCollection<Drive>(drives);
            _dbHelper = DependencyService.Get<IDbHelper>();
            _dialogService = DependencyService.Get<IDialogService>();
        }

        private async Task Unfriend()
        {
            bool response = await _dialogService.ShowMessage($"Are you sure you want to remove {Friend.FullName} as your friend?",
                                                             "Remove Friend", "Yes", "No", null);
            if (response)
            {
                await _dbHelper.DeleteFriend(new DeleteFriendRequest
                {
                    Username = Friend.Username
                });

                var friends = (await _dbHelper.GetPersonFriends(new GetPersonFriendsRequest
                {
                    Username = MainPage.Person.Username
                })).Friends.Select(o => (Friend)o).ToList();

                MainPage.Person.Friends = friends;

                await PopupNavigation.Instance.PopAsync();
            }
        }
    }
}