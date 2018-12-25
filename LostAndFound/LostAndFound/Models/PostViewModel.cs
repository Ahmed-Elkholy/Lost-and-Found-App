using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LostAndFound.Models
{
    public class PostViewModel
    {
        public bool LF { get; set; }
        public string Descr { get; set; }
        public int CID { get; set; }
        public byte[] Photo { get; set; }
    }
}