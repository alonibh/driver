using Driver.Models;
using Driver.ViewModels;
using System;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class NewDriveSummaryPage : ContentPage
    {
        public NewDriveSummaryPage(Drive drive)
        {
            InitializeComponent();

            var viewModel = new NewDriveSummaryViewModel(drive, Navigation);
            BindingContext = viewModel;

            datePicker.MaximumDate = DateTime.Today;
            datePicker.Date = drive.Date;
            datePicker.DateSelected += viewModel.OnDateSelected;

            destEntry.TextChanged += viewModel.OnDestChanged;
        }
    }
}