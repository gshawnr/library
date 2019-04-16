using System;
using System.Collections.Generic;

namespace LittleLibrary.Models
{
    public partial class UsersBooks
    {
        public string UserName { get; set; }
        public int UserBookId { get; set; }
        public int? UserId { get; set; }
        public int? BookId { get; set; }
        public string Review { get; set; }
        public DateTime? ReviewDate { get; set; }
        public bool? IsPurchased { get; set; }
        public short? Rating { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PaypalEmail { get; set; }
        public string amount { get; set; }
        public string intent { get; set; }
        public string paymentMethod { get; set; }
        public string paymentState { get; set; }
        public string paymentId { get; set; }

        public Books Book { get; set; }
        public Users User { get; set; }
    }
}
