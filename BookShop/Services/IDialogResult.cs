using System;

namespace BookShop.Services
{
    public interface IDialogResult
    {
        event EventHandler<RequestCloseDialogEventArgs> RequestCloseDialog;
    }
}
