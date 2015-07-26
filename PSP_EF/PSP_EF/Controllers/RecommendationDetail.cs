using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSP_EF.Models
{
    public class RecommendationDetail
    {

        public virtual Recommendation recommendation { get; set; }
        public string Name_CustomerA { get; set; }
        public string Name_CustomerB { get; set; }

    }
}