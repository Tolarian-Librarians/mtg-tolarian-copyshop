using MahApps.Metro.Controls.Dialogs;
using System;
using System.Threading.Tasks;
using Tolarian.Copyshop.ScreenPresenter.Views;
using static MahApps.Metro.SimpleChildWindow.ChildWindowManager;

namespace Tolarian.Copyshop.ScreenPresenter.Communication
{
    public class Dialogs
    {
        #region Fields

        private readonly CopyShopView _DialogWindow;

        #endregion

        #region ctor

        public Dialogs()
        {
            this._DialogWindow = CopyShopView.GetInstance();
        }

        #endregion

        #region Methods

        public void SendErrorMessage(string errorMessage)
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                this.ShowMessage("Error", errorMessage);
            }
        }

        #endregion

        #region Base Dialogs

        internal async void ShowMessage(string header, string message)
            => await System.Windows.Application.Current.Dispatcher.Invoke(() =>  this._DialogWindow.ShowMessageAsync(header, message).ConfigureAwait(false));

        internal MessageDialogResult ShowQuestion(string header, string message, MessageDialogStyle style,
            string affirmativeButtonText = "YES", string negativeButtonText = "NO", string firstAuxiliaryButtonText = "CANCEL", string secondAuxiliaryButtonText = "")
            => _DialogWindow.ShowModalMessageExternal(header, message, style,
                 new MetroDialogSettings()
                 {
                     AffirmativeButtonText = affirmativeButtonText,
                     NegativeButtonText = negativeButtonText,
                     FirstAuxiliaryButtonText = firstAuxiliaryButtonText,
                     SecondAuxiliaryButtonText = secondAuxiliaryButtonText,
                     DialogResultOnCancel = MessageDialogResult.Canceled,
                     DefaultButtonFocus = MessageDialogResult.Affirmative
                 });

        internal void ShowProgress(string header, string message, Action FunctionWhileProgress) 
            => System.Windows.Application.Current.Dispatcher.Invoke(() => this.ShowProgressOnWindowThread(header, message, FunctionWhileProgress));

        private async void ShowProgressOnWindowThread(string header, string message, Action FunctionWhileProgress)
        {
            // Show...
            ProgressDialogController controller = await CopyShopView.GetInstance().ShowProgressAsync(header, message).ConfigureAwait(true);
            controller.SetIndeterminate();

            // Do your work...
            await Task.Run(FunctionWhileProgress).ConfigureAwait(true);

            // Close...
            await controller.CloseAsync().ConfigureAwait(true);
        }

        #endregion
    }
}
