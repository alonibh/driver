using Driver.Models;
using Driver.ViewModels;
using System.Collections.Generic;

namespace Driver.Views
{
    public partial class FriendPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public FriendPopupPage(Friend friend, List<Drive> drives)
        {
            InitializeComponent();
            BindingContext = new FriendPopupViewModel(friend, drives);
        }
    }
}