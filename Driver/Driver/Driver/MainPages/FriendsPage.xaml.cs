﻿using Driver.Models;
using Driver.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Driver.MainPages
{
    public partial class FriendsPage : ContentPage
    {
        public FriendsPage(IEnumerable<Friend> friends, string username)
        {
            InitializeComponent();

            var viewModel = new FriendsPageViewModel(new ObservableCollection<Friend>(friends), username);
            this.BindingContext = viewModel;
        }
    }
}