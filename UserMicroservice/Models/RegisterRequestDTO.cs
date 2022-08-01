using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
namespace UserManagement.Models
{
    public class RegisterRequestDTO
    {
      //  [Required]
       // public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }
       
        [Required]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }

       
    }
}
