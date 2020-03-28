using GalaSoft.MvvmLight.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Driver.Helpers
{
    public class DialogService : IDialogService
    {
        public async Task ShowError(string message, string title, string buttonText, Action afterHideCallback = null)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, buttonText);

            afterHideCallback?.Invoke();
        }

        public async Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback = null)
        {
            await Application.Current.MainPage.DisplayAlert(title, error.Message, buttonText);

            afterHideCallback?.Invoke();
        }

        public async Task ShowMessage(string message, string title)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }

        public async Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback = null)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, buttonText);

            afterHideCallback?.Invoke();
        }

        public async Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText, Action<bool> afterHideCallback = null)
        {
            var result = await Application.Current.MainPage.DisplayAlert(title, message, buttonConfirmText, buttonCancelText);

            afterHideCallback?.Invoke(result);
            return result;
        }

        public async Task ShowMessageBox(string message, string title)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }
    }
}