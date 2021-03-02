using Microsoft.AspNetCore.Identity;

namespace DAL.Models.EntityModels
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
