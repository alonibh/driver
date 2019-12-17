using Driver.DB;
using Driver.Login;
using System;
using System.IO;
using Xamarin.Forms;

namespace Driver
{
    public partial class App : Application
    {
        static IDb database;

        public static IDb Database
        {
            get
            {
                if (database == null)
                {
                    database = new LocalDb(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Users.db3"));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
