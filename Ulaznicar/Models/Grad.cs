namespace Ulaznicar.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Grad")]
    public partial class Grad
    {
        public Grad()
        {
            Lokacija = new HashSet<Lokacija>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string imegrad { get; set; }

        public virtual ICollection<Lokacija> Lokacija { get; set; }
    }
}
