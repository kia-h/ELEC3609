using System;
using System.Collections.Generic;


namespace PSP_EF.Models
{
    public class SearchServiceResult
    {
        public int service_id { get; set; }
        public int service_provider_id { get; set; }
        public string service_type { get; set; }
        public string service_provider_name { get; set; }
        public string service_desc { get; set; }
        public decimal service_price { get; set; }
    }
}
