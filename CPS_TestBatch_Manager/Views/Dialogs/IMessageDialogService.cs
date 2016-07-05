namespace CPS_TestBatch_Manager.Views.Dialogs
{
    public interface IMessageDialogService
    {
        MessageDialogResult ShowYesNoDialog(string title, string text, MessageDialogResult defaultResult = MessageDialogResult.Yes);
        MessageDialogResult ShowOkDialog(string title, string text, MessageDialogResult defaultResult = MessageDialogResult.Ok);
    }
}
