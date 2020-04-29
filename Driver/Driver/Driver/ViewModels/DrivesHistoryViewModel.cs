using Driver.Models;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Driver.ViewModels
{
    public class DrivesHistoryViewModel : BaseViewModel
    {
        public ObservableCollection<ObservableDrive> Drives { get; set; }

        public DrivesHistoryViewModel(IEnumerable<Drive> drives)
        {
            var observableDrives = new List<ObservableDrive>();
            foreach (var drive in drives)
            {
                observableDrives.Add(new ObservableDrive
                {
                    Id = drive.Id,
                    Driver = drive.Driver,
                    Date = drive.Date,
                    Destination = drive.Destination,
                    Participants = new ObservableCollection<DriveParticipant>(drive.Participants.Where(o => o.Username != drive.Driver.Username))
                });
            }
            Drives = new ObservableCollection<ObservableDrive>(observableDrives.OrderByDescending(o => o.Date));
        }
    }
    public class ObservableDrive
    {
        public string Id { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
        public ObservableCollection<DriveParticipant> Participants { get; set; }
        public DriveParticipant Driver { get; set; }
    }
}