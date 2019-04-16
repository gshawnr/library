using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LittleLibrary.ViewModels
{
    public class CheckoutVM
    {
        [Key]
        public string Id { get; set; }
        public string Email { get; set; }
    }
}
