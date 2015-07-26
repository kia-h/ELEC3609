using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSP_EF.Models
{
    public class SearchLostPetResult
    {
        public int lostInfoId { get; set; }
        public string pet_owner_name { get; set; }
        public string pet_category { get; set; }
        public string city { get; set; }
        public string pet_desc { get; set; }
    }
}