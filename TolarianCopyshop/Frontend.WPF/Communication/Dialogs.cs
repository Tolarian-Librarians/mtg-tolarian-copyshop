using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Threading.Tasks;
using System.Windows;
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
                this.ShowMessageOnUIThread("Error", errorMessage);
            }
        }

        #endregion

        #region Child Window

        internal async Task<T> ShowChildWindowOnUIThread<T>(ChildWindow childWindow) 
            => await Application.Current.Dispatcher.Invoke(() => CopyShopView.GetInstance().ShowChildWindowAsync<T>(childWindow).ConfigureAwait(false));

        #endregion

        #region Base Dialogs

        internal async void ShowMessageOnUIThread(string header, string message)
            => await Application.Current.Dispatcher.Invoke(() => this._DialogWindow.ShowMessageAsync(header, message).ConfigureAwait(false));

        internal MessageDialogResult ShowQuestionOnUIThread(string header, string message, MessageDialogStyle style,
            string affirmativeButtonText = "YES", string negativeButtonText = "NO", string firstAuxiliaryButtonText = "CANCEL", string secondAuxiliaryButtonText = "")
            => Application.Current.Dispatcher.Invoke(() => this.ShowQuestion(header, message, style,
                affirmativeButtonText, negativeButtonText, firstAuxiliaryButtonText, secondAuxiliaryButtonText));

        private MessageDialogResult ShowQuestion(string header, string message, MessageDialogStyle style,
            string affirmativeButtonText, string negativeButtonText, string firstAuxiliaryButtonText, string secondAuxiliaryButtonText)
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

        internal void ShowProgressOnUIThread(string header, string message, Action FunctionWhileProgress)
            => Application.Current.Dispatcher.Invoke(() => this.ShowProgress(header, message, FunctionWhileProgress));

        private async void ShowProgress(string header, string message, Action FunctionWhileProgress)
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
