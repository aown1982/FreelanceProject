//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tUOMRelationship
    {
        public int ID { get; set; }
        public int MasterID { get; set; }
        public int SubID { get; set; }
    
        public virtual tUnitsOfMeasure tUnitsOfMeasure { get; set; }
        public virtual tUnitsOfMeasure tUnitsOfMeasure1 { get; set; }
    }
}
