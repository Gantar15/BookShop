using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookShop.Models.Base
{
    public class Model : INotifyPropertyChanged 
    {
        public virtual event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
