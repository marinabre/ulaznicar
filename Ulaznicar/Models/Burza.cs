//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ulaznicar.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Burza
    {
        public int Id { get; set; }
        public Nullable<int> IdKupac { get; set; }
        public int IdKarta { get; set; }
        public System.DateTime datumponude { get; set; }
        public Nullable<System.DateTime> datumprodaje { get; set; }
        public decimal cijena { get; set; }
    
        public virtual KupljeneKarte KupljeneKarte { get; set; }
        public virtual Korisnik Korisnik { get; set; }
    }
}
