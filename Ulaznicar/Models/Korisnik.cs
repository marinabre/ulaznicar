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
    
    public partial class Korisnik
    {
        public Korisnik()
        {
            this.Burza = new HashSet<Burza>();
            this.KupljeneKarte = new HashSet<KupljeneKarte>();
        }
    
        public int Id { get; set; }
        public string korisnickoime { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public string email { get; set; }
        public string OIB { get; set; }
        public int IdGrad { get; set; }
    
        public virtual ICollection<Burza> Burza { get; set; }
        public virtual ICollection<KupljeneKarte> KupljeneKarte { get; set; }
    }
}
