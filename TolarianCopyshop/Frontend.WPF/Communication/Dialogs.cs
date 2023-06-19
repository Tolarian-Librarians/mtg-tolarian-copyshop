using System;
using System.Threading.Tasks;
using System.Windows;

using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;

using Tolarian.Copyshop.Fontend.WPF.Views;

using static MahApps.Metro.SimpleChildWindow.ChildWindowManager;

namespace Tolarian.Copyshop.Fontend.WPF.Communication
{
    public class Dialogs
    {
        #region Fields

        private readonly CopyShopView _DialogWindow;

        #endregion

        #region ctor

        public Dialogs()
        {
            _DialogWindow = CopyShopView.GetInstance();
        }

        #endregion

        #region Methods

        public void SendErrorMessage(string errorMessage)
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ShowMessageOnUIThread("Error", errorMessage);
            }
        }

        #endregion

        #region Child Window

        internal async Task<T> ShowChildWindowOnUIThread<T>(ChildWindow childWindow)
            => await Application.Current.Dispatcher.Invoke(() => CopyShopView.GetInstance().ShowChildWindowAsync<T>(childWindow).ConfigureAwait(false));

        #endregion

        #region Base Dialogs

        internal async void ShowMessageOnUIThread(string header, string message)
            => await Application.Current.Dispatcher.Invoke(() => _DialogWindow.ShowMessageAsync(header, message).ConfigureAwait(false));

        internal MessageDialogResult ShowQuestionOnUIThread(string header, string message, MessageDialogStyle style,
            string affirmativeButtonText = "YES", string negativeButtonText = "NO", string firstAuxiliaryButtonText = "CANCEL", string secondAuxiliaryButtonText = "")
            => Application.Current.Dispatcher.Invoke(() => ShowQuestion(header, message, style,
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

        internal async Task<string> ShowInputOnUIThread(string header, string message)
            => await _DialogWindow.ShowInputAsync(header, message).ConfigureAwait(false);

        internal void ShowProgressOnUIThread(string header, string message, Action FunctionWhileProgress, Action FunctionAfterProgress = null)
            => Application.Current.Dispatcher.Invoke(() => ShowProgress(header, message, FunctionWhileProgress, FunctionAfterProgress));

        private async void ShowProgress(string header, string message, Action FunctionWhileProgress, Action FunctionAfterProgress)
        {
            // Show...
            ProgressDialogController controller = await CopyShopView.GetInstance().ShowProgressAsync(header, message).ConfigureAwait(true);
            controller.SetIndeterminate();

            // Do your work...
            await Task.Run(FunctionWhileProgress).ConfigureAwait(true);

            // Close...
            await controller.CloseAsync().ConfigureAwait(true);

            if (FunctionAfterProgress != null)
            {
                await Task.Run(FunctionAfterProgress).ConfigureAwait(true);
            }
        }

        private ProgressDialogController progressController;
        public async void StartProgress(string header, string message)
        {
            if (progressController is null)
            {
                progressController = await CopyShopView.GetInstance().ShowProgressAsync(header, message).ConfigureAwait(true);
                progressController.SetIndeterminate();
            }
        }

        public async void EndProgress()
        {
            if (progressController != null)
            {
                await progressController.CloseAsync().ConfigureAwait(true);
                progressController = null;
            }
        }

        #endregion
    }
}