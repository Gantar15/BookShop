using BookShop.Infrastructure.Commands;

namespace BookShop.ViewModels.Base
{
    public abstract class AdminPageViewModel : ViewModel
    {
        public abstract LambdaCommand _searchCommand { get; set; }
        public abstract LambdaCommand SearchCommand { get; }
    }
}
