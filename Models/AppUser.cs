using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using  Microsoft.AspNetCore.Identity;

namespace University.Models {
    
public class AppUser: IdentityUser
    {
         
        public string Name { get; set; }
                 
        [Required]
        [EmailAddress]
         public string Email { get; set; }
  
        [Required]
        public string Password { get; set; }
    }
}