namespace Ulaznicar.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Dogadjaj")]
    public partial class Dogadjaj
    {
        public Dogadjaj()
        {
            Karta = new HashSet<Karta>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string naziv { get; set; }

        public DateTime datum { get; set; }

        public int IdLokacija { get; set; }

        public int brojmjesta { get; set; }

        public virtual Lokacija Lokacija { get; set; }

        public virtual ICollection<Karta> Karta { get; set; }
    }
}
