//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LostAndFound.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reply
    {
        public int RID { get; set; }
        public int PID { get; set; }
        public int UID { get; set; }
        public System.DateTime RDate { get; set; }
        public string Descr { get; set; }
    
        public virtual Post Post { get; set; }
    }
}
