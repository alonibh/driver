using Driver.Models;
using MvvmHelpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Driver.ViewModels
{
    public class DrivesHistoryViewModel : BaseViewModel
    {
        public ObservableCollection<Drive> Drives { get; set; }

        public DrivesHistoryViewModel(IEnumerable<Drive> drives)
        {
            Drives = new ObservableCollection<Drive>(drives);
        }
    }
}