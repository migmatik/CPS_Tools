using System.Windows;
using System.Windows.Media;

namespace CPS_TestBatch_Manager.Views.Dialogs
{
    public class MessageDialogService : IMessageDialogService
    {
        public MessageDialogResult ShowYesNoDialog(string title, string text, MessageDialogResult defaultResult = MessageDialogResult.Yes)
        {
            var dlg = new MessageDialog(title, text, defaultResult, MessageDialogResult.Yes, MessageDialogResult.No);
            dlg.Owner = Application.Current.MainWindow;
            return dlg.ShowDialog();
        }

        public MessageDialogResult ShowOkDialog(string title, string text, MessageDialogResult defaultResult = MessageDialogResult.Ok)
        {
            var dlg = new MessageDialog(title, text, defaultResult, MessageDialogResult.Ok);
            dlg.Background = Brushes.Red;
            dlg.Owner = Application.Current.MainWindow;
            return dlg.ShowDialog();
        }
    }
}
