using Driver.Helpers;
using Driver.Models;
using GalaSoft.MvvmLight.Views;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class MainPage : MasterDetailPage
    {
        public static Person Person { get; set; }

        readonly MasterPage _masterPage;
        readonly IDbHelper _dbHelper;
        readonly IDialogService _dialogService;

        public MainPage(Person person)
        {
            InitializeComponent();

            Person = person;
            _masterPage = new MasterPage();
            _dbHelper = DependencyService.Get<IDbHelper>();
            _dialogService = DependencyService.Get<IDialogService>();

            Master = _masterPage;
            Detail = new NavigationPage(new HomePage())
            {
                BarBackgroundColor = Color.FromRgb(115, 81, 199),
                BarTextColor = Color.Black
            };

            _masterPage.listView.ItemSelected += OnItemSelected;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                var navigationStack = Detail.Navigation.NavigationStack;
                if (item.TargetType != navigationStack[navigationStack.Count - 1].GetType())
                {
                    switch (item.TargetType.Name)
                    {
                        case nameof(HomePage):
                            {
                                await ((NavigationPage)Detail).PopToRootAsync();
                                break;
                            }
                        case nameof(LoginPage):
                            {
                                await Logout();
                                break;
                            }
                        case nameof(FriendsTabbedPage):
                            {
                                await ((NavigationPage)Detail).PushAsync(new FriendsTabbedPage());
                                break;
                            }
                        case nameof(NewDriveParticipantsPage):
                            {
                                await ((NavigationPage)Detail).PushAsync(new NewDriveParticipantsPage());
                                break;
                            }
                        case nameof(DrivesHistoryPage):
                            {
                                await ((NavigationPage)Detail).PushAsync(new DrivesHistoryPage());
                                break;
                            }
                    }
                }
                _masterPage.listView.SelectedItem = null;
                IsPresented = false;
            }
        }

        async Task Logout()
        {
            bool answer = await _dialogService.ShowMessage("Are you sure you want to logout?", "Logout", "Yes", "No", null);
            if (answer)
            {
                _dbHelper.SetToken(null);
                Application.Current.Properties.Remove("username");
                Application.Current.Properties.Remove("token");

                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
}