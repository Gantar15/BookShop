using BookShop.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.ViewModels
{
    class RegisterViewModel
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        RegisterForm Form = new RegisterForm();
    }
}
