namespace Ulaznicar.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Karta")]
    public partial class Karta
    {
        public Karta()
        {
            KupljeneKarte = new HashSet<KupljeneKarte>();
        }

        public int Id { get; set; }

        public int IdVrstakarte { get; set; }

        public decimal cijena { get; set; }

        public int IdDogadjaj { get; set; }

        public virtual Dogadjaj Dogadjaj { get; set; }

        public virtual VrstaKarte VrstaKarte { get; set; }

        public virtual ICollection<KupljeneKarte> KupljeneKarte { get; set; }
    }
}
