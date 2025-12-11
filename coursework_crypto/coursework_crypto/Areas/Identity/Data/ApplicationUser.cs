using System.ComponentModel.DataAnnotations.Schema;
using CryptoTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace CryptoTracker.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {

        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

        public virtual ICollection<UserFavorite> Favorites { get; set; }
    }
}