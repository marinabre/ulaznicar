namespace Ulaznicar.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Burza")]
    public partial class Burza
    {
        public int Id { get; set; }

        public int? IdKupac { get; set; }

        public int IdKarta { get; set; }

        public DateTime datumponude { get; set; }

        public DateTime? datumprodaje { get; set; }

        public decimal cijena { get; set; }

        public virtual Korisnik Korisnik { get; set; }
    }
}
