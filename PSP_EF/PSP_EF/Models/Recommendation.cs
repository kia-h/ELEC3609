using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSP_EF.Models
{
    public class Recommendation
    {
        public int RecommendationId { get; set; }
        public int CustomerAid { get; set; }
        public int CustomerBid { get; set; }
        public int CustomerCid { get; set; }
        public string Description { get; set; }
        public bool Isread { get; set; }

        public virtual Customer Customer { get; set; }
    }
}