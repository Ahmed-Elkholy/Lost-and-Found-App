using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LostAndFound.Models
{
    public class PostViewModel
    {
        [Required]
        public bool LF { get; set; }
        [Required]
        public string Descr { get; set; }
        public int CID { get; set; }
    }
}