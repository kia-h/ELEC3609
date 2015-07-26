using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSP_EF.Models
{
    public class LF_Comment
    {
        public int CommentId { get; set; }
        public int InfoId { get; set; }
        public int CommenterId { get; set; }
        public string Comment { get; set; }
    }
}