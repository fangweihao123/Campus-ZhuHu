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
    
    public partial class SKILL_TO_STUDENT
    {
        public int S_ID { get; set; }
        public string SKILL_NAME { get; set; }
    
        public virtual SKILL SKILL { get; set; }
    }
}
