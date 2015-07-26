using System;
using System.Collections.Generic;

namespace PSP_EF.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<Rating> Rating { get; set; }

    }
}