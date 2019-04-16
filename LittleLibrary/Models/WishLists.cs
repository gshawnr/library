using System;
using System.Collections.Generic;

namespace LittleLibrary.Models
{
    public partial class WishLists
    {
        public int WishListId { get; set; }
        public int? UserId { get; set; }
        public int? BookId { get; set; }

        public Books Book { get; set; }
        public Users User { get; set; }
    }
}
