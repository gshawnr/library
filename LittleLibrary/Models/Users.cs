using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LittleLibrary.Models
{
    public partial class Users
    {
        public Users()
        {
            UsersBooks = new HashSet<UsersBooks>();
        }

        [Key]
        public int UserId { get; set; }
        public string authorizedUserId { get; set; }
        public string Email { get; set; }



        public ICollection<UsersBooks> UsersBooks { get; set; }
    }
}
