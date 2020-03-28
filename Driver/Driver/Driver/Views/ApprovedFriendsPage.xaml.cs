using Driver.Models;
using Driver.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class ApprovedFriendsPage : ContentPage
    {
        public ApprovedFriendsPage(IEnumerable<Friend> friends, string username)
        {
            InitializeComponent();
            BindingContext = new FriendsViewModel(new ObservableCollection<Friend>(friends), username);
        }
    }
}