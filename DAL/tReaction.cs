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
    
    public partial class tReaction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tReaction()
        {
            this.tUserAllergies = new HashSet<tUserAllergy>();
            this.tXrefReactionsCodes = new HashSet<tXrefReactionsCode>();
            this.tReactions1 = new HashSet<tReaction>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public int ReactionTypeID { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public Nullable<int> ParentID { get; set; }
    
        public virtual tReactionType tReactionType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUserAllergy> tUserAllergies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tXrefReactionsCode> tXrefReactionsCodes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tReaction> tReactions1 { get; set; }
        public virtual tReaction tReaction1 { get; set; }
    }
}
