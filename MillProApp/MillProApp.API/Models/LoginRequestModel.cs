using System.ComponentModel.DataAnnotations;

namespace MillProApp.API.Models
{
    public class LoginRequestModel
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Password { get; set; }
        
    }
}