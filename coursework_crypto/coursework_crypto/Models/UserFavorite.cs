using CryptoTracker.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace CryptoTracker.Models
{
    public class UserFavorite
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CoinId { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}