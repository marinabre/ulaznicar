namespace Ulaznicar.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KupljeneKarte")]
    public partial class KupljeneKarte
    {
        public int Id { get; set; }

        public int IdKorisnik { get; set; }

        public int IdKarta { get; set; }

        public virtual Karta Karta { get; set; }

        public virtual Korisnik Korisnik { get; set; }
    }
}
