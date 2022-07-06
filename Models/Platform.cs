using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlatformService.Models
{
    public class Platform 
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = default!;
        
        [Required]
        public string Publisher { get; set; } = default!;
        
        [Required]
        public string Cost { get; set; } = default!;
    }
}