using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace PSP_EF.Models
{
    public class Adoption
    {
        public int AdoptionId{get;set;}
        public int CustomerId {get ; set; }
        public DateTime? AdoptDate { get; set; }
        public string status { get; set; }
        
    }
}