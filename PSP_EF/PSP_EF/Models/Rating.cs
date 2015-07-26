using System.Collections.Generic;
using System.Web.Mvc;
namespace PSP_EF.Models
{
    public class Rating
    {
        public int RatingId { get; set; }
        public int Rate { get; set; }
        public bool Type { get; set; }
        public string Comment { get; set; }
        public int TransactionId { get; set; }

        //public virtual Customer Customer { get; set; }
        public virtual Transaction Transaction { get; set; }

        //public Transaction Transaction { get; set; }
        //public List<SelectListItem> ratelist { get; set; }

    }
}