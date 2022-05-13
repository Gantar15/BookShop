using BookShop.Models.Validation;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class OrderForm : ValidateModel
    {
        private string _fio;
        private string _address;
        private string _phone;

        public OrderForm()
        {
            Validate();
        }

        [Required(ErrorMessage = "ФИО обязательно")]
        [RegularExpression(@"^([А-Яа-яA-Za-z]+ ){2}[А-Яа-яA-Za-z]+$", ErrorMessage = "Некорректное ФИО")]
        public string Fio
        {
            get =>  _fio;
            set => Set(ref _fio, value);
        }

        [Required(ErrorMessage = "Адрес обязателен")]
        [RegularExpression(@"^([a-zA-Zа-яА-Я0-9]+[\.,\s]){5,12}[a-zA-Zа-яА-Я0-9]+$", ErrorMessage = "Некорректный адрес")]
        public string Address
        {
            get => _address;
            set => Set(ref _address, value);
        }

        [Required(ErrorMessage = "Телефон обязателен")]
        [RegularExpression(@"^\+375(\d{2,3})\d{7}$", ErrorMessage = "Некорректный телефон (Код +375)")]
        public string Phone
        {
            get => _phone;
            set => Set(ref _phone, value);
        }
    }
}
