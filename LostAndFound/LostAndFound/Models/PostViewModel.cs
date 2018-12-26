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
        [MaxLength(10000, ErrorMessage = "You reached 10000 characters"), MinLength(5, ErrorMessage = "Please write a longer description")]
        public string Descr { get; set; }
        [Required]
        public int CID { get; set; }
    }
}