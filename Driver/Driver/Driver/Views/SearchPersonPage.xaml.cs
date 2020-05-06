using Driver.ViewModels;
using Xamarin.Forms;

namespace Driver.Views
{
    public partial class SearchPersonPage : ContentPage
    {
        public SearchPersonPage()
        {
            InitializeComponent();

            ToolbarItems.Add(new ToolbarItem
            {
                IconImageSource = ImageSource.FromFile("close.png"),
                Order = ToolbarItemOrder.Primary,
                Priority = 0,
                Command = new Command(async () => { await Navigation.PopModalAsync(); })
            });
            SearchPersonViewModel viewModel = new SearchPersonViewModel();
            BindingContext = viewModel;
            searchBar.TextChanged += viewModel.OnTextChanged;
        }
    }
}