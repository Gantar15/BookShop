using BookShop.ViewModels.Base;

namespace BookShop.ViewModels
{
    public class DialogViewModel : ViewModel
    {
        public ViewModel ShowingViewModel { get; set; }
        public DialogViewModel(ViewModel vm)
        {
            ShowingViewModel = vm;
        }
    }
}
