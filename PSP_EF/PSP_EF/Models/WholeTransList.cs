using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSP_EF.Models
{
    public class WholeTransList
    {
        public IEnumerable<TransList> CustomerTranslist { get; set; }
        public IEnumerable<TransList> ProviderTranslist { get; set; } 
    }
}