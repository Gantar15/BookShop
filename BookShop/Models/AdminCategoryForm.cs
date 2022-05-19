using BookShop.Models.Base;
using DataAccess;

namespace BookShop.Models
{
    public class AdminCategoryForm : Model
    {
        private Category _category;
        private string _image;

        public AdminCategoryForm()
        {
            Image = @"\Imgs\slider_bg.jpg";
            Category = new();
        }

        public string Image
        {
            get => _image;
            set => Set(ref _image, value);
        }
        public Category Category
        {
            get => _category;
            set => Set(ref _category, value);
        }
    }
}
