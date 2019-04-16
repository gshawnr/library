using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleLibrary.ViewModels
{
   
    public class PaymentVM
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string bookTitle { get; set; }
        public string amount { get; set; }
        public string payPalEmail { get; set; }
        public int bookID { get; set; }
        public bool isPurchased { get; set; }
        public string paymentId { get; set; }
        public string paymentState { get; set; }
        public string intent { get; set; }

    }
}