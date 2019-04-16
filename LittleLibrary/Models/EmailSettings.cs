using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleLibrary.Models
{
       public class EmailSettings
    {
        public string Domain { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string UsernamePassword { get; set; }

        public string FromEmail { get; set; }

        public string ToEmail { get; set; }

        public string CCEmail { get; set; }
    }

}
