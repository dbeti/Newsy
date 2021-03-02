using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Newsy.Models.BindingModels
{
    public class RegisterBindingModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAuthor { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
