using System;

namespace BookShop.Services
{
    public class RequestCloseDialogEventArgs : EventArgs
    {
        public bool DialogResult { get; set; }
        public RequestCloseDialogEventArgs(bool DialogResult)
        {
            this.DialogResult = DialogResult;
        }
    }
}
