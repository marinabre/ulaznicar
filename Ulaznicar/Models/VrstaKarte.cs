namespace Ulaznicar.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VrstaKarte")]
    public partial class VrstaKarte
    {
        public VrstaKarte()
        {
            Karta = new HashSet<Karta>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string imevrste { get; set; }

        public virtual ICollection<Karta> Karta { get; set; }
    }
}
