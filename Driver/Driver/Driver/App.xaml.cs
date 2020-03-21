using Driver.API;
using Driver.API.Dbo;
using Driver.DB;
using Driver.LoginPages;
using Driver.MainPages;
using Driver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Driver
{
    public partial class App : Application
    {
        static IDb _database;

        public static IDb Database
        {
            get
            {
                if (_database == null)
                {
                    //database = new LocalDb(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Users.db3"));
                    _database = new RemoteDb("http://roeij.com:3000/api/");
                    if (Current.Properties.ContainsKey("token"))
                    {
                        string token = Current.Properties["token"].ToString();
                        _database.SetToken(token);
                    }
                }
                return _database;
            }
        }

        public App()
        {
            InitializeComponent();

            if (Current.Properties.ContainsKey("token") && Current.Properties.ContainsKey("username"))
                MainPage = new LoadingPage();
            else
                MainPage = new NavigationPage(new LoginPage());
        }

        protected override async void OnStart()
        {
            if (Current.Properties.ContainsKey("token") && Current.Properties.ContainsKey("username"))
            {
                string token = Current.Properties["token"].ToString();
                string username = Current.Properties["username"].ToString();

                Database.SetToken(token);

                PersonDbo person;
                List<DriveDbo> drives;
                try
                {
                    person = (await Database.GetPerson(new GetPersonRequest
                    {
                        Username = username
                    })).Person;

                    drives = (await Database.GetPersonDrives(new GetPersonDrivesRequest
                    {
                        Username = username
                    })).Drives;
                }
                catch (Exception)
                {
                    MainPage = new NavigationPage(new LoginPage());
                    return;
                }

                MainPage mainPage = new MainPage()
                {
                    BindingContext = new Person
                    {
                        Username = person.Username,
                        Address = person.Address,
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        Email = person.Email,
                        Drives = drives.Select(o => (Drive)o).ToList(),
                        Friends = new List<Friend>()
                    }
                };

                MainPage = new NavigationPage(mainPage);
            }
        }

        protected override void OnSleep()
        {
            _database.Dispose();
            _database = null;
        }

        protected override void OnResume()
        {
        }
    }
}