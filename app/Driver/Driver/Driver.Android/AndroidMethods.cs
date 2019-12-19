using Android.OS;
using Driver.Droid;
using Driver.MainPages;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidMethods))]
namespace Driver.Droid
{
    public class AndroidMethods : IAndroidMethods
    {
        public void CloseApp()
        {
            Process.KillProcess(Process.MyPid());
        }
    }
}