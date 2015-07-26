using System;
using System.Collections.Generic;


namespace PSP_EF.Models
{
    public class SearchPetResult
    {
        public int service_id { get; set; }
        public string pet_owner_name { get; set; }
        public string pet_category { get; set; }
        public string pet_desc { get; set; }
        public decimal pet_price { get; set; }
    }
}
