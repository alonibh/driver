using Driver.Views;
using MvvmHelpers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Driver.ViewModels
{
    public class FriendsTabbedViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;

        public ICommand OnSearchPersonToolbarItemClicked => new Command(async () => await OpenSearchPersonModal());

        public FriendsTabbedViewModel(INavigation navigation)
        {
            _navigation = navigation;
        }

        async Task OpenSearchPersonModal()
        {
            await _navigation.PushModalAsync(new SearchPersonPage());
        }
    }
}