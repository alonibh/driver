using Driver.API;
using Driver.API.Dbo;
using Driver.DB;
using Driver.Helpers;
using Driver.Models;
using Driver.Views;
using GalaSoft.MvvmLight.Views;
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
                    _database = new RemoteDb("http://driverapp-dev.ddns.net:3000/api/");
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

            DependencyService.Register<IDbHelper, RemoteDbHelper>();
            DependencyService.Register<IDialogService, DialogService>();

            if (Current.Properties.ContainsKey("token") && Current.Properties.ContainsKey("username"))
            {
                MainPage = new LoadingPage();
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        protected override async void OnStart()
        {
            if (Current.Properties.ContainsKey("token") && Current.Properties.ContainsKey("username"))
            {
                string token = Current.Properties["token"].ToString();
                string username = Current.Properties["username"].ToString();

                Database.SetToken(token);

                PersonDbo personDbo;
                List<DriveDbo> drives;
                List<FriendDbo> friends;

                try
                {
                    personDbo = (await Database.GetPerson(new GetPersonRequest
                    {
                        Username = username
                    })).Person;

                    drives = (await Database.GetPersonDrives(new GetPersonDrivesRequest
                    {
                        Username = username
                    })).Drives;

                    friends = (await Database.GetPersonFriends(new GetPersonFriendsRequest
                    {
                        Username = username
                    })).Friends;
                }
                catch (Exception)
                {
                    MainPage = new NavigationPage(new LoginPage());
                    return;
                }

                Person person = new Person
                {
                    Username = personDbo.Username,
                    Address = personDbo.Address,
                    FirstName = personDbo.FirstName,
                    LastName = personDbo.LastName,
                    Email = personDbo.Email,
                    Drives = drives.Select(o => (Drive)o).ToList(),
                    Friends = friends.Select(o => (Friend)o).ToList()
                };

                MainPage = new MainPage(person);
            }
        }

        protected override void OnSleep()
        {
            _database?.Dispose();
            _database = null;
        }

        protected override void OnResume()
        {
        }
    }
}