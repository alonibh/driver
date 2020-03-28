using Driver.Models;
using Driver.ViewModels;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class NewDriveParticipantsPage : ContentPage
    {
        public NewDriveParticipantsPage(Drive drive, ObservableCollection<Friend> observableFriends)
        {
            InitializeComponent();
            BindingContext = new NewDriveParticipantsViewModel(drive, observableFriends, Navigation);
        }
    }
}