//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VONE
{
    using System;
    using System.Collections.Generic;
    
    public partial class WANTED_POS
    {
        public int POS_ID { get; set; }
        public string POS_NAME { get; set; }
        public Nullable<int> SALARY { get; set; }
        public string POS_DESCRIPTION { get; set; }
    
        public virtual POS_RELATED_FIELD POS_RELATED_FIELD { get; set; }
    }
}
