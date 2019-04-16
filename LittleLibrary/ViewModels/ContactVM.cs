using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LittleLibrary.ViewModels
{
    public class ContactVM
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string textBox { get; set; }

        [EmailAddress(ErrorMessage = "Valid Email Address is required")]
        [Required]
        public string _email { get; set; }
    }
}
