﻿
using Driver.Models;
using Driver.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class DrivesHistoryPage : ContentPage
    {
        public DrivesHistoryPage(IEnumerable<Drive> drives)
        {
            InitializeComponent();
            BindingContext = new DrivesHistoryViewModel(drives);
        }
    }
}