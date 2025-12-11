using CryptoTracker.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTracker.Models
{
    public class UserAsset
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CoinId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 10)")] 
        public decimal Amount { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}